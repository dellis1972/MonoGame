## Plan: Implement MGG_OpenGL Native Backend

The OpenGL backend (`native/monogame/opengl/MGG_OpenGL.cpp`) has 52 MGG functions defined but only ~8 issue real GL calls (device lifecycle, clear, present, viewport, scissor). The remaining ~44 are stubs. The goal is to implement all stubs using OpenGL 4.1/4.3 (macOS/Linux) and WebGL2 (Emscripten), following patterns from the Vulkan backend.

A critical discovery: the OpenGL4 shader profile produces **the same binary container format as Vulkan's** — Vulkan-style descriptor layout metadata prepended to GLSL 330 source text. This means `MGG_Shader_Create` can reuse the Vulkan parsing logic, then `glCompileShader` instead of `vkCreateShaderModule`.

The approach keeps everything in a single file and targets Linux/macOS + Emscripten (Windows uses DX12). No new dependencies — `GL_GLEXT_PROTOTYPES` + SDL2 remain sufficient.

** Building **

Use the following premake5 command to build the desktop GL backend:

You need to `cd` into the `native/monogame` which is where the premake5.lua script is located.

```bash
VULKAN_SDK=~/VulkanSDK/1.4.304.0/macOS premake5 gmake 2>&1
```

```bash
VULKAN_SDK=~/VulkanSDK/1.4.304.0/macOS make desktopgl config=debug 2>&1
```

You can also build the Emscripten target with:

```bash
source ../emsdk/emsdk_env.sh
```

```bash
VULKAN_SDK=~/VulkanSDK/1.4.304.0/macOS premake5 gmake --os=emscripten 2>&1
```

```bash
VULKAN_SDK=~/VulkanSDK/1.4.304.0/macOS make config=debug 2>&1
```

Be sure to build after each step to catch any compilation errors early. The final goal is to have a fully implemented OpenGL backend that passes all tests and achieves visual parity with the Vulkan backend.

**Steps**

### Step 1: Enum Conversion Helpers

Add ~15 static conversion functions at the top of `native/monogame/opengl/MGG_OpenGL.cpp`, translating MonoGame enums to GL enums. These are needed by every subsequent step. Model on the Vulkan backend's `ToVk*` functions (starting at `native/monogame/vulkan/MGG_Vulkan.cpp`, line ~400):

- `ToGLPrimitiveType` — `MGPrimitiveType` → `GL_TRIANGLES`, `GL_TRIANGLE_STRIP`, `GL_LINES`, `GL_LINE_STRIP`
- `ToGLTextureTarget` — `MGTextureType` → `GL_TEXTURE_2D`, `GL_TEXTURE_3D`, `GL_TEXTURE_CUBE_MAP`
- `ToGLBufferTarget` — `MGBufferType` → `GL_ARRAY_BUFFER`, `GL_ELEMENT_ARRAY_BUFFER`, `GL_UNIFORM_BUFFER`
- `ToGLInternalFormat` / `ToGLFormat` / `ToGLType` — `MGSurfaceFormat` → GL format triplet (e.g., `GL_RGBA8`/`GL_RGBA`/`GL_UNSIGNED_BYTE`). Handle compressed formats (`GL_COMPRESSED_*`).
- `ToGLDepthFormat` — `MGDepthFormat` → `GL_DEPTH_COMPONENT16`, `GL_DEPTH24_STENCIL8`
- `ToGLBlendFactor` — `MGBlend` → `GL_ONE`, `GL_SRC_ALPHA`, `GL_ONE_MINUS_SRC_ALPHA`, etc.
- `ToGLBlendOp` — `MGBlendFunction` → `GL_FUNC_ADD`, `GL_FUNC_SUBTRACT`, etc.
- `ToGLCompareFunc` — `MGCompareFunction` → `GL_NEVER`, `GL_LESS`, `GL_ALWAYS`, etc.
- `ToGLStencilOp` — `MGStencilOperation` → `GL_KEEP`, `GL_REPLACE`, `GL_INCR`, etc.
- `ToGLFillMode` — `MGFillMode` → `GL_FILL`, `GL_LINE` (desktop only, no-op on WebGL)
- `ToGLCullMode` — `MGCullMode` → `GL_BACK`, `GL_FRONT`
- `ToGLWrapMode` — `MGTextureAddressMode` → `GL_REPEAT`, `GL_CLAMP_TO_EDGE`, `GL_MIRRORED_REPEAT`
- `ToGLMinFilter` / `ToGLMagFilter` — `MGTextureFilter` → `GL_NEAREST`, `GL_LINEAR`, `GL_LINEAR_MIPMAP_LINEAR`, etc.
- `ToGLVertexAttribType` — `MGVertexElementFormat` → size + type + normalized tuple for `glVertexAttribPointer`

### Step 2: Update Struct Definitions

Add missing GL handles and dirty-tracking fields to the existing structs in `native/monogame/opengl/MGG_OpenGL.cpp`:

- **`MGG_Buffer`** — add `GLuint handle` (for `glGenBuffers`), `GLenum target`
- **`MGG_Texture`** — already has `GLuint texture`; add `GLenum target` (from `ToGLTextureTarget`)
- **`MGG_Shader`** — add parsed slot bitmasks (`uniformSlots`, `textureSlots`, `samplerSlots`), compiled `GLuint shader` handle, and descriptor binding info (reuse the Vulkan container header parsing)
- **`MGG_OcclusionQuery`** — add `GLuint query`
- **`MGG_GraphicsDevice`** — add:
  - `GLuint defaultVAO` (one global VAO for the device)
  - Current bound state pointers: `MGG_BlendState*`, `MGG_DepthStencilState*`, `MGG_RasterizerState*`
  - Shader slots: `MGG_Shader* shaders[2]` (vertex + pixel)
  - Bound resources: `MGG_Buffer* constantBuffers[N]`, `MGG_Texture* textures[N]`, `MGG_SamplerState* samplers[N]`, `MGG_Buffer* vertexBuffers[N]`, `MGG_Buffer* indexBuffer`
  - `MGG_InputLayout* inputLayout`
  - Current render targets: `MGG_Texture* renderTargets[N]`, `GLuint fbo`, `mgint renderTargetCount`
  - Dirty flags: `bool shaderDirty`, `bool blendDirty`, `bool depthStencilDirty`, `bool rasterizerDirty`, `uint32_t textureDirty`, `uint32_t samplerDirty`, `uint32_t uniformDirty`, `bool inputLayoutDirty`
  - Swapchain info: `mgint backbufferWidth`, `mgint backbufferHeight`
- Add a **program cache**: `std::unordered_map<uint64_t, GLuint>` keyed by `(vertexShaderID | pixelShaderID << 32)`, and a `GLuint currentProgram`

### Step 3: Buffers

Implement the 4 buffer functions using `glGenBuffers`/`glDeleteBuffers`/`glBufferData`/`glBufferSubData`/`glGetBufferSubData`:

- **`MGG_Buffer_Create`** — `glGenBuffers(1, &handle)`, bind and allocate with `glBufferData(target, size, nullptr, GL_DYNAMIC_DRAW)`. Set `target` from `ToGLBufferTarget`.
- **`MGG_Buffer_Destroy`** — `glDeleteBuffers(1, &handle)`
- **`MGG_Buffer_SetData`** — bind → `glBufferSubData(target, offset, size, data)`. On `discard == true`, reallocate with `glBufferData` (orphaning pattern) to avoid stalls.
- **`MGG_Buffer_GetData`** — bind → `glGetBufferSubData`. On WebGL/ES, use `glMapBufferRange` with `GL_MAP_READ_BIT` instead (since `glGetBufferSubData` doesn't exist in ES 3.0).

### Step 4: Textures

Implement the 5 texture functions:

- **`MGG_Texture_Create`** — `glGenTextures(1, &handle)`, bind to `target`, allocate storage with `glTexStorage2D`/`glTexStorage3D` (immutable storage, GL 4.2+ / ES 3.0). Set `target` from `ToGLTextureTarget`. For cube maps: `GL_TEXTURE_CUBE_MAP`, storage is `glTexStorage2D(GL_TEXTURE_CUBE_MAP, mipmaps, internalFormat, w, h)`.
- **`MGG_RenderTarget_Create`** — create as a regular texture via `MGG_Texture_Create`, then set `isRenderTarget = true`. If `depthFormat != None`, create a separate depth renderbuffer: `glGenRenderbuffers`, `glRenderbufferStorage` with the appropriate depth format.
- **`MGG_Texture_Destroy`** — `glDeleteTextures`. If it has a depth renderbuffer, `glDeleteRenderbuffers`.
- **`MGG_Texture_SetData`** — bind → `glTexSubImage2D`/`glTexSubImage3D` (or `glCompressedTexSubImage*` for compressed formats). For cube maps, target face = `GL_TEXTURE_CUBE_MAP_POSITIVE_X + slice`.
- **`MGG_Texture_GetData`** — desktop: `glGetTexImage`. WebGL/ES: attach texture to a temp FBO → `glReadPixels`.

### Step 5: Sampler States

- **`MGG_SamplerState_Create`** — `glGenSamplers(1, &sampler)`, then set parameters: `glSamplerParameteri` for `GL_TEXTURE_WRAP_S/T/R`, `GL_TEXTURE_MIN_FILTER`, `GL_TEXTURE_MAG_FILTER`, `GL_TEXTURE_MAX_ANISOTROPY` (if supported), `GL_TEXTURE_COMPARE_MODE`/`GL_TEXTURE_COMPARE_FUNC` (for shadow maps).
- **`MGG_SamplerState_Destroy`** — `glDeleteSamplers(1, &sampler)`

### Step 6: Shaders and Program Linking

This is the most complex step but the bytecode container format is identical to Vulkan's.

**`MGG_Shader_Create`:**
1. Parse the binary header from `bytecode` — same layout as Vulkan's shader parsing in `native/monogame/vulkan/MGG_Vulkan.cpp`: read `uniformCount`, `uniformSlots`, `textureSlots`, `samplerSlots`, `bindingCount`, skip over the `VkDescriptorSetLayoutBinding` array.
2. The remaining bytes are **GLSL 330 source text**.
3. `glCreateShader(stage == Vertex ? GL_VERTEX_SHADER : GL_FRAGMENT_SHADER)`
4. `glShaderSource(shader, 1, &source, &length)`
5. `glCompileShader(shader)` — check `GL_COMPILE_STATUS`, log errors via `glGetShaderInfoLog`.
6. Store compiled `GLuint`, slot bitmasks, and original bytecode in the `MGG_Shader` struct.

**Program cache** (helper function called at draw time):
1. When `shaderDirty` is set, compute key from `vertexShader`/`pixelShader` pair.
2. Look up in `programCache`. If miss: `glCreateProgram()`, `glAttachShader(vs)`, `glAttachShader(fs)`, `glLinkProgram()`, check `GL_LINK_STATUS`.
3. After linking, query uniform block indices with `glGetUniformBlockIndex` and bind them: `glUniformBlockBinding(program, blockIndex, slot)`.
4. Query sampler/texture uniform locations and set them to appropriate texture units with `glUniform1i`.
5. Cache and call `glUseProgram(program)`.

**`MGG_Shader_Destroy`** — `glDeleteShader(handle)`. Invalidate any program cache entries referencing this shader (delete linked programs with `glDeleteProgram`).

### Step 7: Input Layout (VAO)

- **`MGG_InputLayout_Create`** — store the `elements[]` and `strides[]` arrays (as the current code already does). No GL object created here — VAO attribute setup happens at bind time.
- **`MGG_InputLayout_Destroy`** — free the struct.
- At draw time (applied lazily): iterate `elements`, call `glEnableVertexAttribArray(location)`, `glVertexAttribPointer(location, size, type, normalized, stride, offset)` with the appropriate parameters from `ToGLVertexAttribType`.

The device should use a single default VAO created in `MGG_GraphicsDevice_Create` and bound for the device lifetime. Vertex attribute state is reconfigured on this VAO when the input layout or vertex buffer changes.

### Step 8: State Application

Implement the three state-setting functions to issue real GL calls. Use dirty flags to defer application to draw time.

**`MGG_GraphicsDevice_SetBlendState`:**
- Store state pointer + blend factor, mark `blendDirty`.
- At draw time: `glEnable(GL_BLEND)` or `glDisable(GL_BLEND)`. Per-target (4 targets via `glBlendFuncSeparatei` / `glBlendEquationSeparatei` if GL 4.0+), or single-target on WebGL. Set `glBlendColor(r,g,b,a)`. Apply color write mask via `glColorMaski`.

**`MGG_GraphicsDevice_SetDepthStencilState`:**
- Store state pointer, mark `depthStencilDirty`.
- At draw time: `glEnable/glDisable(GL_DEPTH_TEST)`, `glDepthFunc`, `glDepthMask`. If stencil enabled: `glEnable(GL_STENCIL_TEST)`, `glStencilFuncSeparate`, `glStencilOpSeparate`, `glStencilMask`.

**`MGG_GraphicsDevice_SetRasterizerState`:**
- Store state pointer, mark `rasterizerDirty`.
- At draw time: `glEnable/glDisable(GL_CULL_FACE)`, `glCullFace`, `glFrontFace`. Desktop only: `glPolygonMode(GL_FRONT_AND_BACK, fillMode)`. If scissor test enable flag: `glEnable/glDisable(GL_SCISSOR_TEST)`. Depth bias: `glEnable(GL_POLYGON_OFFSET_FILL)`, `glPolygonOffset(factor, units)`. MSAA: `glEnable/glDisable(GL_MULTISAMPLE)`.

### Step 9: Resource Binding

Implement the resource binding functions. These store references and mark dirty bits — actual GL binding is deferred to draw time.

- **`MGG_GraphicsDevice_SetConstantBuffer`** — store `buffer` at `slot` for `stage`, mark `uniformDirty`.
- **`MGG_GraphicsDevice_SetTexture`** — store `texture` at `slot` for `stage`, mark `textureDirty`.
- **`MGG_GraphicsDevice_SetSamplerState`** — store `state` at `slot` for `stage`, mark `samplerDirty`.
- **`MGG_GraphicsDevice_SetIndexBuffer`** — store index buffer handle and element size.
- **`MGG_GraphicsDevice_SetVertexBuffer`** — store `buffer` at `slot` with `vertexOffset`, mark `inputLayoutDirty`.
- **`MGG_GraphicsDevice_SetShader`** — store `shader` at `stage`, mark `shaderDirty`.
- **`MGG_GraphicsDevice_SetInputLayout`** — store `layout`, mark `inputLayoutDirty`.

### Step 10: Draw Calls

Implement a shared `ApplyState` helper called before every draw, then the three draw functions:

**`ApplyState(device, primitiveType)` helper:**
1. If `shaderDirty`: look up / create linked program, `glUseProgram`.
2. If `blendDirty`: apply blend state GL calls.
3. If `depthStencilDirty`: apply depth/stencil GL calls.
4. If `rasterizerDirty`: apply rasterizer GL calls.
5. If `uniformDirty`: for each active UBO slot, `glBindBufferBase(GL_UNIFORM_BUFFER, slot, buffer->handle)`.
6. If `textureDirty`: for each active texture slot, `glActiveTexture(GL_TEXTURE0 + slot)`, `glBindTexture(target, texture->handle)`.
7. If `samplerDirty`: for each active sampler slot, `glBindSampler(slot, sampler->sampler)`.
8. If `inputLayoutDirty`: reconfigure VAO vertex attributes — for each element, bind the associated VBO (`glBindBuffer(GL_ARRAY_BUFFER, vbo)`), then `glVertexAttribPointer`.
9. If index buffer is set: `glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, indexBuffer->handle)`.
10. Clear all dirty flags.

**`MGG_GraphicsDevice_Draw`** — `ApplyState`, then `glDrawArrays(topology, vertexStart, vertexCount)`.

**`MGG_GraphicsDevice_DrawIndexed`** — `ApplyState`, compute index count from `primitiveCount` + `primitiveType`, then `glDrawElements(topology, indexCount, indexType, offset)`.

**`MGG_GraphicsDevice_DrawIndexedInstanced`** — `ApplyState`, then `glDrawElementsInstanced(topology, indexCount, indexType, offset, instanceCount)`.

### Step 11: Render Targets (FBO)

**`MGG_GraphicsDevice_SetRenderTargets`:**
- If `count == 0` (default framebuffer): `glBindFramebuffer(GL_FRAMEBUFFER, 0)`, set viewport to backbuffer size.
- Otherwise: create/cache an FBO. For each target, `glFramebufferTexture2D(GL_FRAMEBUFFER, GL_COLOR_ATTACHMENT0 + i, target, texture->handle, 0)`. If the first target has a depth renderbuffer, attach it. Call `glDrawBuffers` with the attachment list. Check `glCheckFramebufferStatus`.
- Use `arraySlices` parameter for layered rendering (`glFramebufferTextureLayer` for 3D/array textures, or `GL_TEXTURE_CUBE_MAP_POSITIVE_X + slice` for cube faces).

**`MGG_GraphicsDevice_ResolveRenderTargets`** — `glBlitFramebuffer` from MSAA FBO to resolve FBO (if multisampling), or no-op for non-MSAA targets.

**`MGG_GraphicsDevice_GetBackBufferData`** — bind default framebuffer, `glReadPixels(x, y, width, height, GL_RGBA, GL_UNSIGNED_BYTE, data)`.

### Step 12: Occlusion Queries

- **`MGG_OcclusionQuery_Create`** — `glGenQueries(1, &query)`
- **`MGG_OcclusionQuery_Destroy`** — `glDeleteQueries(1, &query)`
- **`MGG_OcclusionQuery_Begin`** — `glBeginQuery(GL_SAMPLES_PASSED, query)` (desktop) or `GL_ANY_SAMPLES_PASSED` (WebGL2)
- **`MGG_OcclusionQuery_End`** — `glEndQuery(GL_SAMPLES_PASSED)`
- **`MGG_OcclusionQuery_GetResult`** — `glGetQueryObjectuiv(query, GL_QUERY_RESULT_AVAILABLE, &available)`, if available: `glGetQueryObjectuiv(query, GL_QUERY_RESULT, &pixelCount)`, return 1; else return 0.

### Step 13: Effect Bytecodes

Uncomment the effect bytecode includes at the top of `native/monogame/opengl/MGG_OpenGL.cpp` and enable `mg_effect.h`. The `.ogl.mgfxo.h` files are generated by the build system (`build/BuildShaders/BuildShadersOGL4Task.cs`). Once uncommented, `MGG_EffectResource_GetBytecode` will work via the shared implementation in `native/monogame/include/mg_effect.h`.

### Step 14: Device Lifecycle Cleanup

Tighten up the existing implemented functions:
- **`MGG_GraphicsDevice_Create`** — create the default VAO, initialize dirty flags, set initial GL state (`glEnable(GL_DEPTH_TEST)`, `glEnable(GL_BLEND)`, etc.).
- **`MGG_GraphicsDevice_Destroy`** — delete the default VAO, destroy the program cache (delete all linked programs), clean up FBO cache.
- **`MGG_GraphicsDevice_ResizeSwapchain`** — avoid destroying and recreating the GL context (current code does this destructively). Instead, just update the viewport/scissor and any swapchain-related state.
- **`MGG_GraphicsDevice_GetTitleSafeArea`** — return the backbuffer dimensions (same as what the Vulkan backend does).

## Verification

1. Build the `desktopgl` project via premake5 to confirm compilation with no errors.
2. Run the MonoGame test suite (`Tests/MonoGame.Tests.DesktopGL4.csproj`) — at minimum, device creation + clear + present should work after Steps 1–2.
3. After Steps 3–10, verify with a simple `SpriteBatch` draw (requires SpriteEffect shader, a texture, and a draw call).
4. After all steps, run the full test suite and verify visual parity with the Vulkan backend.

## Decisions

- **GL loader**: Keep `GL_GLEXT_PROTOTYPES` + SDL2 — no new deps. Windows not targeted (uses DX12).
- **Single file**: All code stays in `native/monogame/opengl/MGG_OpenGL.cpp`, matching Vulkan's pattern.
- **Scope**: Both desktop GL 4.1+ and Emscripten/WebGL2 paths, using `#ifdef MG_EMSCRIPTEN` where APIs diverge (e.g., `glGetBufferSubData` → `glMapBufferRange`, `glPolygonMode` → no-op, `GL_SAMPLES_PASSED` → `GL_ANY_SAMPLES_PASSED`).
- **Shader format**: The OpenGL4 bytecode container is identical to Vulkan's header format (slot bitmasks + bindings) followed by GLSL 330 text. Parse the same header, compile GLSL instead of creating SPIR-V modules.
- **Program linking**: Cache linked programs by VS+FS pair ID (same pattern as Vulkan's `MGVK_Program`).
- **State management**: Deferred/dirty-tracked like Vulkan — store state at set time, apply at draw time.
