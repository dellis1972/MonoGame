// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

#include "api_MGG.h"
#include "mg_common.h"

// Effect includes for OpenGL
#include "AlphaTestEffect.ogl.mgfxo.h"
#include "BasicEffect.ogl.mgfxo.h"
#include "DualTextureEffect.ogl.mgfxo.h"
#include "EnvironmentMapEffect.ogl.mgfxo.h"
#include "SkinnedEffect.ogl.mgfxo.h"
#include "SpriteEffect.ogl.mgfxo.h"
#include "mg_effect.h"

// Include required headers for OpenGL/Emscripten
#if defined(MG_EMSCRIPTEN)
#include <emscripten.h>
#include <emscripten/html5.h>
// Include OpenGLES 3.0 headers
#include <GLES3/gl3.h>
#else
// #if defined(__APPLE__)
// #include <OpenGL/gl.h>
// #include <OpenGL/glext.h>
// #else
// #include <GL/gl.h>
// #include <GL/glext.h>
// #endif
#endif

#if defined(MG_SDL2)
#define GL_GLEXT_PROTOTYPES 1
#include <SDL.h>
#include <SDL_opengl.h>
#include <SDL_opengl_glext.h>
#endif

#include <vector>
#include <string>
#include <cstring>
#include <cstdint>
#include <map>
#include <unordered_map>
#include <queue>
#include <optional>

// Debug macros
#ifdef DEBUG
#define GL_CHECK_ERROR() do { \
    GLenum err = glGetError(); \
    if (err != GL_NO_ERROR) { \
        fprintf(stderr, "OpenGL error %d at %s:%d\n", err, __FILE__, __LINE__); \
    } \
} while(0)
#else
#define GL_CHECK_ERROR() ((void)0)
#endif

// ============================================================
// Enum Conversion Helpers — MonoGame enums → GL enums
// ============================================================

static GLenum ToGLPrimitiveType(MGPrimitiveType type)
{
	switch (type)
	{
	case MGPrimitiveType::TriangleList:
		return GL_TRIANGLES;
	case MGPrimitiveType::TriangleStrip:
		return GL_TRIANGLE_STRIP;
	case MGPrimitiveType::LineList:
		return GL_LINES;
	case MGPrimitiveType::LineStrip:
		return GL_LINE_STRIP;
	case MGPrimitiveType::PointList:
		return GL_POINTS;
	default:
		assert(!"Unsupported primitive type!");
		return GL_TRIANGLES;
	}
}

static GLenum ToGLTextureTarget(MGTextureType type)
{
	switch (type)
	{
	case MGTextureType::_2D:
		return GL_TEXTURE_2D;
	case MGTextureType::_3D:
		return GL_TEXTURE_3D;
	case MGTextureType::Cube:
		return GL_TEXTURE_CUBE_MAP;
	default:
		assert(!"Unsupported texture type!");
		return GL_TEXTURE_2D;
	}
}

static GLenum ToGLBufferTarget(MGBufferType type)
{
	switch (type)
	{
	case MGBufferType::Vertex:
		return GL_ARRAY_BUFFER;
	case MGBufferType::Index:
		return GL_ELEMENT_ARRAY_BUFFER;
	case MGBufferType::Constant:
		return GL_UNIFORM_BUFFER;
	default:
		assert(!"Unsupported buffer type!");
		return GL_ARRAY_BUFFER;
	}
}

static GLenum ToGLInternalFormat(MGSurfaceFormat format)
{
	switch (format)
	{
	case MGSurfaceFormat::Color:
		return GL_RGBA8;
	case MGSurfaceFormat::Bgr565:
		return GL_RGB565;
	case MGSurfaceFormat::Bgra5551:
		return GL_RGB5_A1;
	case MGSurfaceFormat::Bgra4444:
		return GL_RGBA4;
	case MGSurfaceFormat::Dxt1:
		return GL_COMPRESSED_RGB_S3TC_DXT1_EXT;
	case MGSurfaceFormat::Dxt3:
		return GL_COMPRESSED_RGBA_S3TC_DXT3_EXT;
	case MGSurfaceFormat::Dxt5:
		return GL_COMPRESSED_RGBA_S3TC_DXT5_EXT;
	case MGSurfaceFormat::NormalizedByte2:
		return GL_RG8;
	case MGSurfaceFormat::NormalizedByte4:
		return GL_RGBA8;
	case MGSurfaceFormat::Rgba1010102:
		return GL_RGB10_A2;
	case MGSurfaceFormat::Rg32:
		return GL_RG16;
	case MGSurfaceFormat::Rgba64:
		return GL_RGBA16;
	case MGSurfaceFormat::Alpha8:
		return GL_R8;
	case MGSurfaceFormat::Single:
		return GL_R32F;
	case MGSurfaceFormat::Vector2:
		return GL_RG32F;
	case MGSurfaceFormat::Vector4:
		return GL_RGBA32F;
	case MGSurfaceFormat::HalfSingle:
		return GL_R16F;
	case MGSurfaceFormat::HalfVector2:
		return GL_RG16F;
	case MGSurfaceFormat::HalfVector4:
		return GL_RGBA16F;
	case MGSurfaceFormat::HdrBlendable:
		return GL_RGBA16F;
	case MGSurfaceFormat::Bgr32:
		return GL_RGBA8; // No native BGR internal format; swizzle if needed
	case MGSurfaceFormat::Bgra32:
		return GL_RGBA8;
	case MGSurfaceFormat::ColorSRgb:
		return GL_SRGB8_ALPHA8;
	case MGSurfaceFormat::Bgr32SRgb:
		return GL_SRGB8_ALPHA8;
	case MGSurfaceFormat::Bgra32SRgb:
		return GL_SRGB8_ALPHA8;
	case MGSurfaceFormat::Dxt1SRgb:
		return GL_COMPRESSED_SRGB_S3TC_DXT1_EXT;
	case MGSurfaceFormat::Dxt3SRgb:
		return GL_COMPRESSED_SRGB_ALPHA_S3TC_DXT3_EXT;
	case MGSurfaceFormat::Dxt5SRgb:
		return GL_COMPRESSED_SRGB_ALPHA_S3TC_DXT5_EXT;
	case MGSurfaceFormat::Dxt1a:
		return GL_COMPRESSED_RGBA_S3TC_DXT1_EXT;
#if defined(MG_EMSCRIPTEN)
	case MGSurfaceFormat::Rgb8Etc2:
		return GL_COMPRESSED_RGB8_ETC2;
	case MGSurfaceFormat::Srgb8Etc2:
		return GL_COMPRESSED_SRGB8_ETC2;
	case MGSurfaceFormat::Rgb8A1Etc2:
		return GL_COMPRESSED_RGB8_PUNCHTHROUGH_ALPHA1_ETC2;
	case MGSurfaceFormat::Srgb8A1Etc2:
		return GL_COMPRESSED_SRGB8_PUNCHTHROUGH_ALPHA1_ETC2;
	case MGSurfaceFormat::Rgba8Etc2:
		return GL_COMPRESSED_RGBA8_ETC2_EAC;
	case MGSurfaceFormat::SRgb8A8Etc2:
		return GL_COMPRESSED_SRGB8_ALPHA8_ETC2_EAC;
#endif
	default:
		assert(!"Unsupported surface format!");
		return GL_RGBA8;
	}
}

static GLenum ToGLFormat(MGSurfaceFormat format)
{
	switch (format)
	{
	case MGSurfaceFormat::Color:
	case MGSurfaceFormat::NormalizedByte4:
	case MGSurfaceFormat::Rgba1010102:
	case MGSurfaceFormat::ColorSRgb:
		return GL_RGBA;
	case MGSurfaceFormat::Bgr565:
		return GL_RGB;
	case MGSurfaceFormat::Bgra5551:
	case MGSurfaceFormat::Bgra4444:
		return GL_RGBA;
	case MGSurfaceFormat::NormalizedByte2:
	case MGSurfaceFormat::Rg32:
		return GL_RG;
	case MGSurfaceFormat::Rgba64:
	case MGSurfaceFormat::HalfVector4:
	case MGSurfaceFormat::Vector4:
	case MGSurfaceFormat::HdrBlendable:
		return GL_RGBA;
	case MGSurfaceFormat::Alpha8:
	case MGSurfaceFormat::Single:
	case MGSurfaceFormat::HalfSingle:
		return GL_RED;
	case MGSurfaceFormat::Vector2:
	case MGSurfaceFormat::HalfVector2:
		return GL_RG;
	case MGSurfaceFormat::Bgr32:
	case MGSurfaceFormat::Bgr32SRgb:
		return GL_BGRA;
	case MGSurfaceFormat::Bgra32:
	case MGSurfaceFormat::Bgra32SRgb:
		return GL_BGRA;
	default:
		assert(!"Unsupported surface format for ToGLFormat!");
		return GL_RGBA;
	}
}

static GLenum ToGLType(MGSurfaceFormat format)
{
	switch (format)
	{
	case MGSurfaceFormat::Color:
	case MGSurfaceFormat::ColorSRgb:
	case MGSurfaceFormat::NormalizedByte2:
	case MGSurfaceFormat::NormalizedByte4:
	case MGSurfaceFormat::Alpha8:
	case MGSurfaceFormat::Bgr32:
	case MGSurfaceFormat::Bgra32:
	case MGSurfaceFormat::Bgr32SRgb:
	case MGSurfaceFormat::Bgra32SRgb:
		return GL_UNSIGNED_BYTE;
	case MGSurfaceFormat::Bgr565:
		return GL_UNSIGNED_SHORT_5_6_5;
	case MGSurfaceFormat::Bgra5551:
		return GL_UNSIGNED_SHORT_5_5_5_1;
	case MGSurfaceFormat::Bgra4444:
		return GL_UNSIGNED_SHORT_4_4_4_4;
	case MGSurfaceFormat::Rgba1010102:
		return GL_UNSIGNED_INT_2_10_10_10_REV;
	case MGSurfaceFormat::Rg32:
	case MGSurfaceFormat::Rgba64:
		return GL_UNSIGNED_SHORT;
	case MGSurfaceFormat::Single:
	case MGSurfaceFormat::Vector2:
	case MGSurfaceFormat::Vector4:
		return GL_FLOAT;
	case MGSurfaceFormat::HalfSingle:
	case MGSurfaceFormat::HalfVector2:
	case MGSurfaceFormat::HalfVector4:
	case MGSurfaceFormat::HdrBlendable:
		return GL_HALF_FLOAT;
	default:
		assert(!"Unsupported surface format for ToGLType!");
		return GL_UNSIGNED_BYTE;
	}
}

static bool IsCompressedFormat(MGSurfaceFormat format)
{
	switch (format)
	{
	case MGSurfaceFormat::Dxt1:
	case MGSurfaceFormat::Dxt3:
	case MGSurfaceFormat::Dxt5:
	case MGSurfaceFormat::Dxt1SRgb:
	case MGSurfaceFormat::Dxt3SRgb:
	case MGSurfaceFormat::Dxt5SRgb:
	case MGSurfaceFormat::Dxt1a:
#if defined(MG_EMSCRIPTEN)
	case MGSurfaceFormat::Rgb8Etc2:
	case MGSurfaceFormat::Srgb8Etc2:
	case MGSurfaceFormat::Rgb8A1Etc2:
	case MGSurfaceFormat::Srgb8A1Etc2:
	case MGSurfaceFormat::Rgba8Etc2:
	case MGSurfaceFormat::SRgb8A8Etc2:
#endif
		return true;
	default:
		return false;
	}
}

static GLenum ToGLDepthFormat(MGDepthFormat format)
{
	switch (format)
	{
	case MGDepthFormat::Depth16:
		return GL_DEPTH_COMPONENT16;
	case MGDepthFormat::Depth24:
		return GL_DEPTH_COMPONENT24;
	case MGDepthFormat::Depth24Stencil8:
		return GL_DEPTH24_STENCIL8;
	default:
		return 0;
	}
}

static GLenum ToGLBlendFactor(MGBlend mode)
{
	switch (mode)
	{
	case MGBlend::One:
		return GL_ONE;
	case MGBlend::Zero:
		return GL_ZERO;
	case MGBlend::SourceColor:
		return GL_SRC_COLOR;
	case MGBlend::InverseSourceColor:
		return GL_ONE_MINUS_SRC_COLOR;
	case MGBlend::SourceAlpha:
		return GL_SRC_ALPHA;
	case MGBlend::InverseSourceAlpha:
		return GL_ONE_MINUS_SRC_ALPHA;
	case MGBlend::DestinationColor:
		return GL_DST_COLOR;
	case MGBlend::InverseDestinationColor:
		return GL_ONE_MINUS_DST_COLOR;
	case MGBlend::DestinationAlpha:
		return GL_DST_ALPHA;
	case MGBlend::InverseDestinationAlpha:
		return GL_ONE_MINUS_DST_ALPHA;
	case MGBlend::BlendFactor:
		return GL_CONSTANT_COLOR;
	case MGBlend::InverseBlendFactor:
		return GL_ONE_MINUS_CONSTANT_COLOR;
	case MGBlend::SourceAlphaSaturation:
		return GL_SRC_ALPHA_SATURATE;
	default:
		assert(!"Unsupported blend mode!");
		return GL_ONE;
	}
}

static GLenum ToGLBlendOp(MGBlendFunction func)
{
	switch (func)
	{
	case MGBlendFunction::Add:
		return GL_FUNC_ADD;
	case MGBlendFunction::Subtract:
		return GL_FUNC_SUBTRACT;
	case MGBlendFunction::ReverseSubtract:
		return GL_FUNC_REVERSE_SUBTRACT;
	case MGBlendFunction::Min:
		return GL_MIN;
	case MGBlendFunction::Max:
		return GL_MAX;
	default:
		assert(!"Unsupported blend function!");
		return GL_FUNC_ADD;
	}
}

static GLenum ToGLCompareFunc(MGCompareFunction func)
{
	switch (func)
	{
	case MGCompareFunction::Always:
		return GL_ALWAYS;
	case MGCompareFunction::Never:
		return GL_NEVER;
	case MGCompareFunction::Less:
		return GL_LESS;
	case MGCompareFunction::LessEqual:
		return GL_LEQUAL;
	case MGCompareFunction::Equal:
		return GL_EQUAL;
	case MGCompareFunction::GreaterEqual:
		return GL_GEQUAL;
	case MGCompareFunction::Greater:
		return GL_GREATER;
	case MGCompareFunction::NotEqual:
		return GL_NOTEQUAL;
	default:
		assert(!"Unsupported compare function!");
		return GL_ALWAYS;
	}
}

static GLenum ToGLStencilOp(MGStencilOperation op)
{
	switch (op)
	{
	case MGStencilOperation::Keep:
		return GL_KEEP;
	case MGStencilOperation::Zero:
		return GL_ZERO;
	case MGStencilOperation::Replace:
		return GL_REPLACE;
	case MGStencilOperation::Increment:
		return GL_INCR_WRAP;
	case MGStencilOperation::Decrement:
		return GL_DECR_WRAP;
	case MGStencilOperation::IncrementSaturation:
		return GL_INCR;
	case MGStencilOperation::DecrementSaturation:
		return GL_DECR;
	case MGStencilOperation::Invert:
		return GL_INVERT;
	default:
		assert(!"Unsupported stencil operation!");
		return GL_KEEP;
	}
}

static GLenum ToGLFillMode(MGFillMode mode)
{
	switch (mode)
	{
#if !defined(MG_EMSCRIPTEN)
	case MGFillMode::Solid:
		return GL_FILL;
	case MGFillMode::WireFrame:
		return GL_LINE;
#else
	// WebGL/ES does not support glPolygonMode
	case MGFillMode::Solid:
	case MGFillMode::WireFrame:
		return GL_FILL;
#endif
	default:
		assert(!"Unsupported fill mode!");
		return GL_FILL;
	}
}

static GLenum ToGLCullMode(MGCullMode mode)
{
	switch (mode)
	{
	case MGCullMode::CullClockwiseFace:
		return GL_FRONT;
	case MGCullMode::CullCounterClockwiseFace:
		return GL_BACK;
	default:
		// MGCullMode::None is handled by disabling GL_CULL_FACE
		return GL_BACK;
	}
}

static GLenum ToGLWrapMode(MGTextureAddressMode mode)
{
	switch (mode)
	{
	case MGTextureAddressMode::Wrap:
		return GL_REPEAT;
	case MGTextureAddressMode::Clamp:
		return GL_CLAMP_TO_EDGE;
	case MGTextureAddressMode::Mirror:
		return GL_MIRRORED_REPEAT;
	case MGTextureAddressMode::Border:
#if !defined(MG_EMSCRIPTEN)
		return GL_CLAMP_TO_BORDER;
#else
		return GL_CLAMP_TO_EDGE; // WebGL2 does not support GL_CLAMP_TO_BORDER
#endif
	default:
		assert(!"Unsupported texture address mode!");
		return GL_REPEAT;
	}
}

static GLenum ToGLMinFilter(MGTextureFilter filter)
{
	switch (filter)
	{
	case MGTextureFilter::Linear:
		return GL_LINEAR_MIPMAP_LINEAR;
	case MGTextureFilter::Point:
		return GL_NEAREST_MIPMAP_NEAREST;
	case MGTextureFilter::Anisotropic:
		return GL_LINEAR_MIPMAP_LINEAR;
	case MGTextureFilter::LinearMipPoint:
		return GL_LINEAR_MIPMAP_NEAREST;
	case MGTextureFilter::PointMipLinear:
		return GL_NEAREST_MIPMAP_LINEAR;
	case MGTextureFilter::MinLinearMagPointMipLinear:
		return GL_LINEAR_MIPMAP_LINEAR;
	case MGTextureFilter::MinLinearMagPointMipPoint:
		return GL_LINEAR_MIPMAP_NEAREST;
	case MGTextureFilter::MinPointMagLinearMipLinear:
		return GL_NEAREST_MIPMAP_LINEAR;
	case MGTextureFilter::MinPointMagLinearMipPoint:
		return GL_NEAREST_MIPMAP_NEAREST;
	default:
		assert(!"Unsupported texture filter!");
		return GL_LINEAR_MIPMAP_LINEAR;
	}
}

static GLenum ToGLMagFilter(MGTextureFilter filter)
{
	switch (filter)
	{
	case MGTextureFilter::Linear:
	case MGTextureFilter::Anisotropic:
	case MGTextureFilter::LinearMipPoint:
		return GL_LINEAR;
	case MGTextureFilter::Point:
	case MGTextureFilter::PointMipLinear:
		return GL_NEAREST;
	case MGTextureFilter::MinLinearMagPointMipLinear:
	case MGTextureFilter::MinLinearMagPointMipPoint:
		return GL_NEAREST;
	case MGTextureFilter::MinPointMagLinearMipLinear:
	case MGTextureFilter::MinPointMagLinearMipPoint:
		return GL_LINEAR;
	default:
		assert(!"Unsupported texture filter!");
		return GL_LINEAR;
	}
}

struct GLVertexAttribInfo {
	GLint size;
	GLenum type;
	GLboolean normalized;
};

static GLVertexAttribInfo ToGLVertexAttribType(MGVertexElementFormat format)
{
	switch (format)
	{
	case MGVertexElementFormat::Single:
		return { 1, GL_FLOAT, GL_FALSE };
	case MGVertexElementFormat::Vector2:
		return { 2, GL_FLOAT, GL_FALSE };
	case MGVertexElementFormat::Vector3:
		return { 3, GL_FLOAT, GL_FALSE };
	case MGVertexElementFormat::Vector4:
		return { 4, GL_FLOAT, GL_FALSE };
	case MGVertexElementFormat::Color:
		return { 4, GL_UNSIGNED_BYTE, GL_TRUE };
	case MGVertexElementFormat::Byte4:
		return { 4, GL_UNSIGNED_BYTE, GL_FALSE };
	case MGVertexElementFormat::Short2:
		return { 2, GL_SHORT, GL_FALSE };
	case MGVertexElementFormat::Short4:
		return { 4, GL_SHORT, GL_FALSE };
	case MGVertexElementFormat::NormalizedShort2:
		return { 2, GL_SHORT, GL_TRUE };
	case MGVertexElementFormat::NormalizedShort4:
		return { 4, GL_SHORT, GL_TRUE };
	case MGVertexElementFormat::HalfVector2:
		return { 2, GL_HALF_FLOAT, GL_FALSE };
	case MGVertexElementFormat::HalfVector4:
		return { 4, GL_HALF_FLOAT, GL_FALSE };
	default:
		assert(!"Unsupported vertex element format!");
		return { 4, GL_FLOAT, GL_FALSE };
	}
}

// ============================================================
// Constants
// ============================================================

static const int MAX_TEXTURE_SLOTS = 16;
static const int MAX_VERTEX_BUFFERS = 8;
static const int MAX_RENDER_TARGETS = 4;
static const int MAX_UNIFORM_BUFFER_SLOTS = 16;

// ============================================================
// Structures for OpenGL graphics system
// ============================================================

struct MGG_GraphicsAdapter {
    // OpenGL specific adapter info
    char name[128] = { 0 };
    char vendor[128] = { 0 };
    char version[128] = { 0 };
};

struct MGG_GraphicsSystem {
    std::vector<MGG_GraphicsAdapter*> adapters;
};

struct MGG_Buffer {
    MGBufferType type = MGBufferType::Vertex;
    GLenum target = GL_ARRAY_BUFFER;
    GLuint handle = 0;
    int sizeInBytes = 0;
};

struct MGG_Texture {
    MGTextureType type = MGTextureType::_2D;
    MGSurfaceFormat format = MGSurfaceFormat::Color;
    GLenum target = GL_TEXTURE_2D;
    int width = 0;
    int height = 0;
    int depth = 0;
    int mipmaps = 1;
    int slices = 0;
    bool isRenderTarget = false;
    MGDepthFormat depthFormat = MGDepthFormat::None;
    MGRenderTargetUsage usage = MGRenderTargetUsage::PlatformContents;
    mgint multiSampleCount = 0;
    GLuint texture = 0;
    GLuint depthRenderbuffer = 0;
};

struct MGG_SamplerState {
    MGG_SamplerState_Info info;
    GLuint sampler = 0;
};

struct MGG_BlendState {
    MGG_BlendState_Info infos[MAX_RENDER_TARGETS];
};

struct MGG_DepthStencilState {
    MGG_DepthStencilState_Info info;
};

struct MGG_RasterizerState {
    MGG_RasterizerState_Info info;
};

// Shader binding descriptor (24 bytes), matching the binary layout
// written by the content pipeline (ShaderProfile.OpenGL4.cs).
struct MGShaderBinding {
    uint32_t binding;          // Binding index
    uint32_t descriptorType;   // See MG_BINDING_TYPE_* constants below
    uint32_t descriptorCount;  // Always 1
    uint32_t stageFlags;       // 0x01 = vertex, 0x10 = fragment
    uint64_t reserved;         // Always 0
};

// Binding descriptor type constants (match values written by the content pipeline)
static const uint32_t MG_BINDING_TYPE_SAMPLER = 0;
static const uint32_t MG_BINDING_TYPE_COMBINED_IMAGE_SAMPLER = 1;
static const uint32_t MG_BINDING_TYPE_SAMPLED_IMAGE = 2;
static const uint32_t MG_BINDING_TYPE_UNIFORM_BUFFER = 8;
static const int MG_TEXTURE_SLOT_OFFSET = 32;

struct MGG_Shader {
    uint32_t id = 0;
    MGShaderStage stage = MGShaderStage::Vertex;
    GLuint shader = 0;

    // Parsed from the bytecode container header
    mguint uniformSlots = 0;
    mguint textureSlots = 0;
    mguint samplerSlots = 0;
    mgint uniformCount = 0;
    mgint bindingCount = 0;

    // Parsed binding info from the header
    std::vector<MGShaderBinding> bindings;

    // Original bytecode kept for reference
    std::vector<uint8_t> bytecode;
};

struct MGG_InputLayout {
    std::vector<MGG_InputElement> elements;
    std::vector<int> strides;
};

struct MGG_OcclusionQuery {
    GLuint query = 0;
    bool isActive = false;
    mgbool isComplete = false;
    mgint pixelCount = 0;
};

struct MGG_GraphicsDevice
{
    MGG_GraphicsSystem* system = nullptr;
    MGG_GraphicsAdapter* adapter = nullptr;

#if defined(MG_EMSCRIPTEN)
    EMSCRIPTEN_WEBGL_CONTEXT_HANDLE context = 0;
#else
    SDL_GLContext context = nullptr;
    SDL_Window* window = nullptr;
#endif

    // Whether the GL context and initial state have been set up.
    bool contextInitialized = false;

    // Default VAO (one global VAO for the device)
    GLuint defaultVAO = 0;

    // Viewport
    int viewportX = 0;
    int viewportY = 0;
    int viewportWidth = 0;
    int viewportHeight = 0;
    float viewportMinDepth = 0.0f;
    float viewportMaxDepth = 1.0f;

    // Scissor rect
    int scissorX = 0;
    int scissorY = 0;
    int scissorWidth = 0;
    int scissorHeight = 0;

    // Swapchain / backbuffer info
    mgint backbufferWidth = 0;
    mgint backbufferHeight = 0;

    // --- Current bound state pointers ---
    MGG_BlendState* blendState = nullptr;
    MGG_DepthStencilState* depthStencilState = nullptr;
    MGG_RasterizerState* rasterizerState = nullptr;

    // Blend factor
    float blendFactor[4] = { 0.0f, 0.0f, 0.0f, 0.0f };
    bool blendFactorDirty = false;

    // --- Shaders ---
    uint32_t currentShaderId = 0;
    MGG_Shader* shaders[(mgint)MGShaderStage::Count] = { nullptr };
    bool shaderDirty = false;
    std::vector<MGG_Shader*> all_shaders;

    // Program cache: keyed by (vertexShaderID | pixelShaderID << 32)
    std::unordered_map<uint64_t, GLuint> programCache;
    GLuint currentProgram = 0;

    // --- Resource bindings ---
    // Constant buffers (per stage)
    MGG_Buffer* constantBuffers[MAX_UNIFORM_BUFFER_SLOTS] = { nullptr };
    uint32_t uniformDirty = 0;

    // Textures and samplers
    MGG_Texture* textures[MAX_TEXTURE_SLOTS] = { nullptr };
    MGG_SamplerState* samplers[MAX_TEXTURE_SLOTS] = { nullptr };
    uint32_t textureDirty = 0;
    uint32_t samplerDirty = 0;

    // Vertex buffers
    MGG_Buffer* vertexBuffers[MAX_VERTEX_BUFFERS] = { nullptr };
    uint32_t vertexOffsets[MAX_VERTEX_BUFFERS] = { 0 };
    uint64_t vertexBuffersDirty = 0xFFFFFFFF;

    // Index buffer
    MGG_Buffer* indexBuffer = nullptr;
    MGIndexElementSize indexBufferSize = MGIndexElementSize::SixteenBits;

    // Input layout
    MGG_InputLayout* inputLayout = nullptr;
    bool inputLayoutDirty = false;

    // --- Dirty flags for state application ---
    bool blendDirty = false;
    bool depthStencilDirty = false;
    bool rasterizerDirty = false;

    // --- Render targets (FBO) ---
    GLuint fbo = 0;
    MGG_Texture* renderTargets[MAX_RENDER_TARGETS] = { nullptr };
    std::optional<int> renderTargetSlices[MAX_RENDER_TARGETS];
    mgint renderTargetCount = 0;
    bool renderTargetDirty = false;

    // --- Tracking for cleanup ---
    std::vector<MGG_Buffer*> all_buffers;
    std::vector<MGG_Texture*> all_textures;
    std::vector<MGG_OcclusionQuery*> deferredOcclusionQueries;
};

// Implementation of API functions

MGG_GraphicsSystem* MGG_GraphicsSystem_Create() {
    printf("Creating OpenGL graphics system\n");
    MGG_GraphicsSystem* system = new MGG_GraphicsSystem();
    
#if defined(MG_EMSCRIPTEN)
    // Create a default adapter
    MGG_GraphicsAdapter* adapter = new MGG_GraphicsAdapter();
    system->adapters.push_back(adapter);
#else
    // Non-Emscripten build - create a dummy adapter for testing
    MGG_GraphicsAdapter* adapter = new MGG_GraphicsAdapter();
    system->adapters.push_back(adapter);
#endif
    return system;
}

void MGG_GraphicsSystem_Destroy(MGG_GraphicsSystem* system) {
    printf("Destroying OpenGL graphics system\n");
    for (auto adapter : system->adapters) {
        delete adapter;
    }
    delete system;
}

MGG_GraphicsAdapter* MGG_GraphicsAdapter_Get(MGG_GraphicsSystem* system, mgint index) {
    printf("Getting OpenGL graphics adapter at index %d\n", index);
    if (!system) return nullptr;
    if (index >= 0 && index < static_cast<mgint>(system->adapters.size())) {
        return system->adapters[index];
    }
    return nullptr;
}

void MGG_GraphicsAdapter_GetInfo(MGG_GraphicsAdapter* adapter, MGG_GraphicsAdaptor_Info& info) {
    assert(adapter);
    printf("Getting info for OpenGL graphics adapter: %s\n", adapter->name);
    // Set adapter properties based on OpenGL capabilities
    // The actual implementation would query these from the OpenGL context
    info.DeviceName = adapter->name;
    info.Description = adapter->vendor;
    info.DeviceId = 0;
    info.Revision = 0;
    info.VendorId = 0;
    info.SubSystemId = 0;
    info.MonitorHandle = nullptr;
    info.DisplayModes = nullptr;
    info.DisplayModeCount = 0;
    // Initialize CurrentDisplayMode with zeros
    info.CurrentDisplayMode = { MGSurfaceFormat::Color, 0, 0 };
}

// Helper: create the GL context and set up initial GL state.
// Called on the first ResizeSwapchain when we have a valid window handle.
#if !defined(MG_EMSCRIPTEN)
static bool MGL_InitContext(MGG_GraphicsDevice* device, SDL_Window* window) {
    device->window = window;

    // Set OpenGL attributes for SDL
    SDL_GL_SetAttribute(SDL_GL_CONTEXT_MAJOR_VERSION, 4);
    SDL_GL_SetAttribute(SDL_GL_CONTEXT_MINOR_VERSION, 3);
    SDL_GL_SetAttribute(SDL_GL_CONTEXT_PROFILE_MASK, SDL_GL_CONTEXT_PROFILE_CORE);
    SDL_GL_SetAttribute(SDL_GL_DOUBLEBUFFER, 1);
    SDL_GL_SetAttribute(SDL_GL_DEPTH_SIZE, 24);
    SDL_GL_SetAttribute(SDL_GL_STENCIL_SIZE, 8);
    SDL_GL_SetAttribute(SDL_GL_ALPHA_SIZE, 8);

    // Create OpenGL context
    device->context = SDL_GL_CreateContext(device->window);
    // If 4.3 fails, try 4.1 (for macOS)
    if (!device->context) {
        printf("OpenGL 4.3 not available, trying 4.1...\n");
        SDL_GL_SetAttribute(SDL_GL_CONTEXT_MAJOR_VERSION, 4);
        SDL_GL_SetAttribute(SDL_GL_CONTEXT_MINOR_VERSION, 1);
        device->context = SDL_GL_CreateContext(device->window);
    }

    if (!device->context) {
        fprintf(stderr, "Failed to create OpenGL context: %s\n", SDL_GetError());
        return false;
    }

    // Make the context current
    if (SDL_GL_MakeCurrent(device->window, device->context) < 0) {
        fprintf(stderr, "Failed to make OpenGL context current: %s\n", SDL_GetError());
        SDL_GL_DeleteContext(device->context);
        device->context = nullptr;
        return false;
    }

    // Print OpenGL version information
    const GLubyte* version = glGetString(GL_VERSION);
    const GLubyte* vendor = glGetString(GL_VENDOR);
    const GLubyte* renderer = glGetString(GL_RENDERER);
    const GLubyte* glslVersion = glGetString(GL_SHADING_LANGUAGE_VERSION);

    printf("=== OpenGL Context Information ===\n");
    printf("OpenGL Version: %s\n", version ? (const char*)version : "Unknown");
    printf("OpenGL Vendor: %s\n", vendor ? (const char*)vendor : "Unknown");
    printf("OpenGL Renderer: %s\n", renderer ? (const char*)renderer : "Unknown");
    printf("GLSL Version: %s\n", glslVersion ? (const char*)glslVersion : "Unknown");
    printf("==================================\n");

    // Create and bind a single default VAO for the device lifetime.
    glGenVertexArrays(1, &device->defaultVAO);
    glBindVertexArray(device->defaultVAO);
    GL_CHECK_ERROR();

    // Create a default FBO for render target usage
    glGenFramebuffers(1, &device->fbo);
    GL_CHECK_ERROR();

    // Set initial GL state
    glEnable(GL_DEPTH_TEST);
    glDepthFunc(GL_LESS);
    glDepthMask(GL_TRUE);
    glEnable(GL_BLEND);
    glBlendFunc(GL_ONE, GL_ZERO);
    glEnable(GL_CULL_FACE);
    glCullFace(GL_BACK);
    glFrontFace(GL_CW);
    glEnable(GL_MULTISAMPLE);
    glEnable(GL_FRAMEBUFFER_SRGB);
    glDisable(GL_SCISSOR_TEST);
    glColorMask(GL_TRUE, GL_TRUE, GL_TRUE, GL_TRUE);
    GL_CHECK_ERROR();

    // Mark all state as dirty so the first draw call applies everything.
    device->blendDirty = true;
    device->depthStencilDirty = true;
    device->rasterizerDirty = true;
    device->shaderDirty = true;
    device->inputLayoutDirty = true;
    device->uniformDirty = 0xFFFFFFFF;
    device->textureDirty = 0xFFFFFFFF;
    device->samplerDirty = 0xFFFFFFFF;
    device->vertexBuffersDirty = 0xFFFFFFFF;
    device->blendFactorDirty = true;
    device->renderTargetDirty = false;

    device->contextInitialized = true;
    printf("Created OpenGL context: %p for window: %p\n", (void*)device->context, (void*)device->window);
    return true;
}
#endif

MGG_GraphicsDevice* MGG_GraphicsDevice_Create(MGG_GraphicsSystem* system, MGG_GraphicsAdapter* adapter) {
    printf("Creating OpenGL graphics device\n");
    MGG_GraphicsDevice* device = new MGG_GraphicsDevice();
    device->system = system;
    device->adapter = adapter;

#if defined(MG_EMSCRIPTEN)
    // Set up OpenGL context attributes
    EmscriptenWebGLContextAttributes attrs;
    emscripten_webgl_init_context_attributes(&attrs);
    attrs.majorVersion = 2; // OpenGL 2.0 maps to OpenGLES 3.0
    attrs.minorVersion = 0;
    attrs.enableExtensionsByDefault = 1;
    attrs.alpha = 1;
    attrs.depth = 1;
    attrs.stencil = 1;
    attrs.antialias = 1;
    attrs.premultipliedAlpha = 1;
    attrs.preserveDrawingBuffer = 0;
    attrs.powerPreference = EM_WEBGL_POWER_PREFERENCE_DEFAULT;
    attrs.failIfMajorPerformanceCaveat = 0;
    printf("Getting canvas\n");
    
    // Get the canvas element - assuming the default "#canvas" selector
    device->context = emscripten_webgl_create_context("#canvas", &attrs);
    printf("Got canvas %lu\n", device->context);
    if (device->context <= 0) {
        fprintf(stderr, "Failed to create OpenGL context: %lu\n", device->context);
        delete device;
        return nullptr;
    }
     printf("making current\n");
    // Make the context current
    EMSCRIPTEN_RESULT result = emscripten_webgl_make_context_current(device->context);
    if (result != EMSCRIPTEN_RESULT_SUCCESS) {
        fprintf(stderr, "Failed to make OpenGL context current: %d\n", result);
        emscripten_webgl_destroy_context(device->context);
        delete device;
        return nullptr;
    }

    // Print OpenGL version information
    const GLubyte* version = glGetString(GL_VERSION);
    const GLubyte* vendor = glGetString(GL_VENDOR);
    const GLubyte* renderer = glGetString(GL_RENDERER);
    const GLubyte* glslVersion = glGetString(GL_SHADING_LANGUAGE_VERSION);
    
    printf("=== OpenGL Context Information ===\n");
    printf("OpenGL Version: %s\n", version ? (const char*)version : "Unknown");
    printf("OpenGL Vendor: %s\n", vendor ? (const char*)vendor : "Unknown");
    printf("OpenGL Renderer: %s\n", renderer ? (const char*)renderer : "Unknown");
    printf("GLSL Version: %s\n", glslVersion ? (const char*)glslVersion : "Unknown");
    printf("==================================\n");

    // Create and bind a single default VAO for the device lifetime.
    glGenVertexArrays(1, &device->defaultVAO);
    glBindVertexArray(device->defaultVAO);
    GL_CHECK_ERROR();

    // Create a default FBO for render target usage
    glGenFramebuffers(1, &device->fbo);
    GL_CHECK_ERROR();

    // Set initial GL state
    glEnable(GL_DEPTH_TEST);
    glDepthFunc(GL_LESS);
    glDepthMask(GL_TRUE);
    glEnable(GL_BLEND);
    glBlendFunc(GL_ONE, GL_ZERO);
    glEnable(GL_CULL_FACE);
    glCullFace(GL_BACK);
    glFrontFace(GL_CW);
    glDisable(GL_SCISSOR_TEST);
    glColorMask(GL_TRUE, GL_TRUE, GL_TRUE, GL_TRUE);
    GL_CHECK_ERROR();

    // Mark all state as dirty so the first draw call applies everything.
    device->blendDirty = true;
    device->depthStencilDirty = true;
    device->rasterizerDirty = true;
    device->shaderDirty = true;
    device->inputLayoutDirty = true;
    device->uniformDirty = 0xFFFFFFFF;
    device->textureDirty = 0xFFFFFFFF;
    device->samplerDirty = 0xFFFFFFFF;
    device->vertexBuffersDirty = 0xFFFFFFFF;
    device->blendFactorDirty = true;
    device->renderTargetDirty = false;
    device->contextInitialized = true;
#else
    // On desktop (SDL), we defer GL context creation to ResizeSwapchain
    // where the actual window handle is provided. This avoids the problem
    // of hardcoding a window ID that becomes invalid when windows are
    // destroyed and recreated (e.g. between test runs).
#endif

    // Initialize default viewport state
    device->viewportX = 0;
    device->viewportY = 0;
    device->viewportWidth = 800;  // Default size
    device->viewportHeight = 600; // Default size
    device->viewportMinDepth = 0.0f;
    device->viewportMaxDepth = 1.0f;
    
    // Initialize default scissor state
    device->scissorX = 0;
    device->scissorY = 0;
    device->scissorWidth = 800;  // Default size
    device->scissorHeight = 600; // Default size

    printf("Created OpenGL graphics device (context deferred): %p\n", (void*)device);
    return device;
}

void MGG_GraphicsDevice_Destroy(MGG_GraphicsDevice* device) {
    if (!device) return;
    printf("Destroying OpenGL graphics device: %zu\n", (size_t)device->context);

    // Delete cached linked programs
    for (auto& pair : device->programCache)
        glDeleteProgram(pair.second);
    device->programCache.clear();
    device->currentProgram = 0;

    // Destroy any remaining buffers tracked by the device
    while (device->all_buffers.size() > 0)
        MGG_Buffer_Destroy(device, device->all_buffers[0]);

    // Destroy any remaining textures tracked by the device
    while (device->all_textures.size() > 0)
        MGG_Texture_Destroy(device, device->all_textures[0]);

    // Destroy deferred occlusion queries
    for (auto* query : device->deferredOcclusionQueries)
    {
        if (query->query != 0)
            glDeleteQueries(1, &query->query);
        delete query;
    }
    device->deferredOcclusionQueries.clear();

    // Destroy any remaining shaders tracked by the device
    for (auto* shader : device->all_shaders)
    {
        if (shader->shader != 0)
            glDeleteShader(shader->shader);
        delete shader;
    }
    device->all_shaders.clear();

    // Delete the FBO used for render targets
    if (device->fbo != 0) {
        glDeleteFramebuffers(1, &device->fbo);
        device->fbo = 0;
    }

    // Delete the default VAO
    if (device->defaultVAO != 0) {
        glDeleteVertexArrays(1, &device->defaultVAO);
        device->defaultVAO = 0;
    }

#if defined(MG_EMSCRIPTEN)
    // Destroy the OpenGL context
    if (device->context > 0) {
        emscripten_webgl_destroy_context(device->context);
        device->context = 0;
    }
#else
    if (device->context) {
        SDL_GL_DeleteContext(device->context);
        device->context = nullptr;
    }
#endif

    delete device;
}

void MGG_GraphicsDevice_GetCaps(MGG_GraphicsDevice* device, MGG_GraphicsDevice_Caps& caps) {
    // Set device capabilities based on OpenGL capabilities
    printf("Getting capabilities for OpenGL graphics device\n");
#if !defined(MG_EMSCRIPTEN)
    if (device->contextInitialized) {
        glGetIntegerv(GL_MAX_TEXTURE_IMAGE_UNITS, &caps.MaxTextureSlots);
        GL_CHECK_ERROR();
        glGetIntegerv(GL_MAX_VERTEX_TEXTURE_IMAGE_UNITS, &caps.MaxVertexTextureSlots);
        GL_CHECK_ERROR();
        glGetIntegerv(GL_MAX_VERTEX_ATTRIBS, &caps.MaxVertexBufferSlots);
        GL_CHECK_ERROR();
    } else {
        // Context not yet created (deferred to ResizeSwapchain).
        // Return safe defaults matching GL 4.1+ minimum guarantees.
        caps.MaxTextureSlots = 16;
        caps.MaxVertexBufferSlots = 16;
        caps.MaxVertexTextureSlots = 16;
    }
#else
    caps.MaxTextureSlots = 16;
    caps.MaxVertexBufferSlots = 8;
    caps.MaxVertexTextureSlots = 8;
#endif
    caps.ShaderProfile = 0; // OpenGL MonoGame Shader Profile
    printf("Device capabilities: MaxTextureSlots=%d, MaxVertexBufferSlots=%d, MaxVertexTextureSlots=%d\n",
           caps.MaxTextureSlots, caps.MaxVertexBufferSlots, caps.MaxVertexTextureSlots);
}

void MGG_GraphicsDevice_ResizeSwapchain(MGG_GraphicsDevice* device, void* nativeWindowHandle, mgint width, mgint height, MGSurfaceFormat color, MGDepthFormat depth, mgint syncInterval) {
    if (!device) return;
    printf("Resizing OpenGL graphics device (width=%d, height=%d)\n", width, height);

#if defined(MG_EMSCRIPTEN)
    // Resize the canvas element to match the requested size.
    if (width > 0 && height > 0) {
        emscripten_set_canvas_element_size("#canvas", width, height);
    }
#else
    // On the first call, create the GL context using the provided window
    // handle.  We defer this from MGG_GraphicsDevice_Create because the
    // window handle is not available there.
    if (!device->contextInitialized) {
        SDL_Window* sdlWindow = (SDL_Window*)nativeWindowHandle;
        if (!sdlWindow) {
            fprintf(stderr, "ResizeSwapchain: nativeWindowHandle is NULL, cannot create GL context\n");
            return;
        }
        if (!MGL_InitContext(device, sdlWindow)) {
            fprintf(stderr, "ResizeSwapchain: Failed to initialize GL context\n");
            return;
        }
    } else if ((SDL_Window*)nativeWindowHandle != device->window) {
        // The window handle changed (shouldn't normally happen, but handle
        // it gracefully by updating the stored pointer and making our
        // context current on the new window).
        device->window = (SDL_Window*)nativeWindowHandle;
        SDL_GL_MakeCurrent(device->window, device->context);
    }

    // Set the VSync interval.  SDL_GL_SetSwapInterval can be called at
    // any time without recreating the context.
    SDL_GL_SetSwapInterval(syncInterval);
#endif
    
    // Store backbuffer dimensions for render target / backbuffer data queries.
    device->backbufferWidth = width;
    device->backbufferHeight = height;

    // Update viewport to match new size
    device->viewportX = 0;
    device->viewportY = 0;
    device->viewportWidth = width;
    device->viewportHeight = height;
    glViewport(0, 0, width, height);
    GL_CHECK_ERROR();
    
    // Update scissor rectangle to match new size
    device->scissorX = 0;
    device->scissorY = 0;
    device->scissorWidth = width;
    device->scissorHeight = height;
    glScissor(0, 0, width, height);
    GL_CHECK_ERROR();
    printf("Resized OpenGL graphics device: %zu\n", (size_t)device->context);
}

mgint MGG_GraphicsDevice_BeginFrame(MGG_GraphicsDevice* device) {
    printf("Beginning frame for OpenGL graphics device\n");
    if (!device) return 0;
    
#if defined(MG_EMSCRIPTEN)
    // Make sure our context is current
    EMSCRIPTEN_RESULT result = emscripten_webgl_make_context_current(device->context);
    if (result != EMSCRIPTEN_RESULT_SUCCESS) {
        fprintf(stderr, "Failed to make OpenGL context current: %d\n", result);
        return 0;
    }
#else
    SDL_GL_MakeCurrent(device->window, device->context);
#endif
    printf("Ending frame for OpenGL graphics device: %zu\n", (size_t)device->context);

    return 1; // Frame index - OpenGL doesn't use multiple frames like Vulkan
}

void MGG_GraphicsDevice_Clear(MGG_GraphicsDevice* device, MGClearOptions options, Vector4& color, mgfloat depth, mgint stencil) {
    printf("Clearing OpenGL graphics device\n");
    if (!device) return;
    
    // Check which framebuffer is currently bound
    GLint currentFramebuffer = 0;
    glGetIntegerv(GL_FRAMEBUFFER_BINDING, &currentFramebuffer);
    printf("Current framebuffer: %d\n", currentFramebuffer);
    
    GLbitfield clearMask = 0;
    
    // Set clear color if color buffer is being cleared
    if (static_cast<mgint>(options) & static_cast<mgint>(MGClearOptions::Target)) {
        glClearColor(color.X, color.Y, color.Z, color.W);
        GL_CHECK_ERROR();
        clearMask |= GL_COLOR_BUFFER_BIT;
        printf("Clear color: (%f, %f, %f, %f)\n", color.X, color.Y, color.Z, color.W);
    }
    
    // Set clear depth if depth buffer is being cleared
    if (static_cast<mgint>(options) & static_cast<mgint>(MGClearOptions::DepthBuffer)) {
#if defined(MG_EMSCRIPTEN)
        // WebGL/OpenGL ES uses glClearDepthf
        glClearDepthf(depth);
#else
        // Desktop OpenGL uses glClearDepth with double parameters
        glClearDepth((double)depth);
#endif
        GL_CHECK_ERROR();
        clearMask |= GL_DEPTH_BUFFER_BIT;
        printf("Clear depth: %f\n", depth);
    }
    
    // Set clear stencil if stencil buffer is being cleared
    if (static_cast<mgint>(options) & static_cast<mgint>(MGClearOptions::Stencil)) {
        glClearStencil(stencil);
        GL_CHECK_ERROR();
        clearMask |= GL_STENCIL_BUFFER_BIT;
        printf("Clear stencil: %d\n", stencil);
    }
    
    // Perform the clear operation
    if (clearMask != 0) {
        printf("Calling glClear with mask: 0x%x\n", clearMask);
        glClear(clearMask);
        GL_CHECK_ERROR();
    }
    
    GL_CHECK_ERROR();

    printf("Cleared OpenGL graphics device\n");
}

void MGG_GraphicsDevice_Present(MGG_GraphicsDevice* device, mgint currentFrame, mgint syncInterval) {
    if (!device) return;
    printf("Presenting OpenGL graphics device: %zu (syncInterval=%d)\n", (size_t)device->context, syncInterval);
   
#if defined(MG_EMSCRIPTEN)
    // In WebGL, the browser handles buffer swapping with requestAnimationFrame
    // We don't need to do anything special here, as opposed to other APIs
    
    // We could potentially use emscripten_set_main_loop_timing to control frame rate
    // But typically the browser's requestAnimationFrame handles this well
#else
    // Ensure we're swapping the correct window
    SDL_Window* currentWindow = SDL_GL_GetCurrentWindow();
    printf("Current SDL window: %p, device window: %p\n", currentWindow, device->window);
    
    if (currentWindow != device->window) {
        fprintf(stderr, "Warning: Current window doesn't match device window!\n");
    }
    
    printf("Calling SDL_GL_SwapWindow...\n");
    SDL_GL_SwapWindow(device->window);
    printf("SDL_GL_SwapWindow completed\n");
#endif
    
    GL_CHECK_ERROR();
    printf("Present completed\n");
}

void MGG_GraphicsDevice_SetBlendState(MGG_GraphicsDevice* device, MGG_BlendState* state, mgfloat factorR, mgfloat factorG, mgfloat factorB, mgfloat factorA) {
    assert(device != nullptr);
    assert(state != nullptr);

    if (device->blendState != state)
    {
        device->blendState = state;
        device->blendDirty = true;
    }

    if (device->blendFactor[0] != factorR ||
        device->blendFactor[1] != factorG ||
        device->blendFactor[2] != factorB ||
        device->blendFactor[3] != factorA)
    {
        device->blendFactor[0] = factorR;
        device->blendFactor[1] = factorG;
        device->blendFactor[2] = factorB;
        device->blendFactor[3] = factorA;
        device->blendFactorDirty = true;
    }
}

void MGG_GraphicsDevice_SetDepthStencilState(MGG_GraphicsDevice* device, MGG_DepthStencilState* state) {
    assert(device != nullptr);
    assert(state != nullptr);

    if (device->depthStencilState != state)
    {
        device->depthStencilState = state;
        device->depthStencilDirty = true;
    }
}

void MGG_GraphicsDevice_SetRasterizerState(MGG_GraphicsDevice* device, MGG_RasterizerState* state) {
    assert(device != nullptr);
    assert(state != nullptr);

    if (device->rasterizerState != state)
    {
        device->rasterizerState = state;
        device->rasterizerDirty = true;
    }
}

void MGG_GraphicsDevice_GetTitleSafeArea(mgint& x, mgint& y, mgint& width, mgint& height) {
    // Nothing for PC here unless we want to support
    // things like Steam TV modes and we need platform
    // specific calls for that.  Matches the Vulkan backend.
    printf("Got title safe area for OpenGL graphics device: (%d, %d, %d, %d)\n", x, y, width, height);
}

void MGG_GraphicsDevice_SetViewport(MGG_GraphicsDevice* device, mgint x, mgint y, mgint width, mgint height, mgfloat minDepth, mgfloat maxDepth) {
    printf("Setting viewport for OpenGL graphics device: %zu (%d, %d, %d, %d)\n", (size_t)device->context, x, y, width, height);
    if (!device) return;
    
    // Store viewport values
    device->viewportX = x;
    device->viewportY = y;
    device->viewportWidth = width;
    device->viewportHeight = height;
    device->viewportMinDepth = minDepth;
    device->viewportMaxDepth = maxDepth;
    
    // Set the viewport in OpenGLES
    glViewport(x, y, width, height);
    GL_CHECK_ERROR();
    
    // Note: OpenGL depth range is [0,1], but Direct3D is [-1,1]
    // minDepth and maxDepth are in the Direct3D range, so we need to map them
#if defined(MG_EMSCRIPTEN)
    // WebGL/OpenGL ES uses glDepthRangef
    glDepthRangef(minDepth, maxDepth);
#else
    // Desktop OpenGL uses glDepthRange with double parameters
    glDepthRange((double)minDepth, (double)maxDepth);
#endif
    GL_CHECK_ERROR();
}

void MGG_GraphicsDevice_SetScissorRectangle(MGG_GraphicsDevice* device, mgint x, mgint y, mgint width, mgint height) {
    if (!device) return;
    printf("Setting scissor rectangle for OpenGL graphics device: %zu (%d, %d, %d, %d)\n", (size_t)device->context, x, y, width, height);
    // Store scissor values
    device->scissorX = x;
    device->scissorY = y;
    device->scissorWidth = width;
    device->scissorHeight = height;
    
    // Enable scissor test
    glEnable(GL_SCISSOR_TEST);
    GL_CHECK_ERROR();
    
    // Set scissor rectangle
    // Note: OpenGL has (0,0) at bottom-left, need to flip Y coordinate
    int flippedY = device->viewportHeight - (y + height);
    glScissor(x, flippedY, width, height);
    
    GL_CHECK_ERROR();
}

void MGG_GraphicsDevice_SetRenderTargets(MGG_GraphicsDevice* device, MGG_Texture** targets, mgint* arraySlices, mgint count) {
    assert(device != nullptr);

    if (targets == nullptr || count == 0)
    {
        // Bind the default framebuffer (backbuffer).
        glBindFramebuffer(GL_FRAMEBUFFER, 0);
        GL_CHECK_ERROR();

        // Clear render target tracking.
        memset(device->renderTargets, 0, sizeof(device->renderTargets));
        device->renderTargetCount = 0;
        for (int i = 0; i < MAX_RENDER_TARGETS; i++)
            device->renderTargetSlices[i] = std::nullopt;

        // Restore viewport to backbuffer size.
        glViewport(device->viewportX, device->viewportY, device->viewportWidth, device->viewportHeight);
#if defined(MG_EMSCRIPTEN)
        glDepthRangef(device->viewportMinDepth, device->viewportMaxDepth);
#else
        glDepthRange((double)device->viewportMinDepth, (double)device->viewportMaxDepth);
#endif
        GL_CHECK_ERROR();
    }
    else
    {
        // Store render target references.
        for (int i = 0; i < count && i < MAX_RENDER_TARGETS; i++)
            device->renderTargets[i] = targets[i];
        for (int i = count; i < MAX_RENDER_TARGETS; i++)
            device->renderTargets[i] = nullptr;
        device->renderTargetCount = count;

        // Store array slices.
        if (arraySlices)
        {
            for (int i = 0; i < MAX_RENDER_TARGETS; i++)
            {
                if (i < count && arraySlices[i] >= 0)
                    device->renderTargetSlices[i] = arraySlices[i];
                else
                    device->renderTargetSlices[i] = std::nullopt;
            }
        }
        else
        {
            for (int i = 0; i < MAX_RENDER_TARGETS; i++)
                device->renderTargetSlices[i] = std::nullopt;
        }

        // Bind the off-screen FBO.
        glBindFramebuffer(GL_FRAMEBUFFER, device->fbo);
        GL_CHECK_ERROR();

        GLenum drawBuffers[MAX_RENDER_TARGETS];
        int drawBufferCount = 0;

        for (int i = 0; i < count && i < MAX_RENDER_TARGETS; i++)
        {
            MGG_Texture* rt = targets[i];
            if (!rt) continue;

            GLenum attachment = GL_COLOR_ATTACHMENT0 + i;
            drawBuffers[drawBufferCount++] = attachment;

            if (rt->type == MGTextureType::Cube && device->renderTargetSlices[i].has_value())
            {
                // Cube map face: GL_TEXTURE_CUBE_MAP_POSITIVE_X + slice
                int slice = device->renderTargetSlices[i].value();
                glFramebufferTexture2D(
                    GL_FRAMEBUFFER,
                    attachment,
                    GL_TEXTURE_CUBE_MAP_POSITIVE_X + slice,
                    rt->texture,
                    0);
            }
            else if (rt->type == MGTextureType::_3D && device->renderTargetSlices[i].has_value())
            {
                // 3D texture or array texture layer.
                int slice = device->renderTargetSlices[i].value();
                glFramebufferTextureLayer(
                    GL_FRAMEBUFFER,
                    attachment,
                    rt->texture,
                    0,
                    slice);
            }
            else
            {
                // Standard 2D texture.
                glFramebufferTexture2D(
                    GL_FRAMEBUFFER,
                    attachment,
                    rt->target,
                    rt->texture,
                    0);
            }
        }

        // Attach depth/stencil from the first render target if it has one.
        if (count > 0 && targets[0] && targets[0]->depthRenderbuffer != 0)
        {
            MGG_Texture* rt0 = targets[0];
            if (rt0->depthFormat == MGDepthFormat::Depth24Stencil8)
            {
                glFramebufferRenderbuffer(GL_FRAMEBUFFER, GL_DEPTH_STENCIL_ATTACHMENT,
                                          GL_RENDERBUFFER, rt0->depthRenderbuffer);
            }
            else
            {
                glFramebufferRenderbuffer(GL_FRAMEBUFFER, GL_DEPTH_ATTACHMENT,
                                          GL_RENDERBUFFER, rt0->depthRenderbuffer);
            }
            GL_CHECK_ERROR();
        }
        else
        {
            // Detach any previously attached depth/stencil.
            glFramebufferRenderbuffer(GL_FRAMEBUFFER, GL_DEPTH_STENCIL_ATTACHMENT,
                                      GL_RENDERBUFFER, 0);
            glFramebufferRenderbuffer(GL_FRAMEBUFFER, GL_DEPTH_ATTACHMENT,
                                      GL_RENDERBUFFER, 0);
            GL_CHECK_ERROR();
        }

        // Set the draw buffer list.
        if (drawBufferCount > 0)
        {
            glDrawBuffers(drawBufferCount, drawBuffers);
            GL_CHECK_ERROR();
        }

        // Validate framebuffer completeness.
        GLenum fbStatus = glCheckFramebufferStatus(GL_FRAMEBUFFER);
        if (fbStatus != GL_FRAMEBUFFER_COMPLETE)
        {
            fprintf(stderr, "MGG_GraphicsDevice_SetRenderTargets: Framebuffer incomplete, status=0x%x\n", fbStatus);
        }

        // Set viewport to the first render target's dimensions.
        if (targets[0])
        {
            glViewport(0, 0, targets[0]->width, targets[0]->height);
#if defined(MG_EMSCRIPTEN)
            glDepthRangef(0.0f, 1.0f);
#else
            glDepthRange(0.0, 1.0);
#endif
            GL_CHECK_ERROR();
        }
    }

    device->renderTargetDirty = false;
}

void MGG_GraphicsDevice_SetConstantBuffer(MGG_GraphicsDevice* device, MGShaderStage stage, mgint slot, MGG_Buffer* buffer) {
    assert(device != nullptr);
    assert(buffer != nullptr);
    assert(slot >= 0 && slot < MAX_UNIFORM_BUFFER_SLOTS);

    if (device->constantBuffers[slot] != buffer)
    {
        device->constantBuffers[slot] = buffer;
        device->uniformDirty |= 1 << slot;
    }
}

void MGG_GraphicsDevice_SetTexture(MGG_GraphicsDevice* device, MGShaderStage stage, mgint slot, MGG_Texture* texture) {
    assert(device != nullptr);
    assert(slot >= 0 && slot < MAX_TEXTURE_SLOTS);

    device->textures[slot] = texture;
    device->textureDirty |= 1 << slot;
}

void MGG_GraphicsDevice_SetSamplerState(MGG_GraphicsDevice* device, MGShaderStage stage, mgint slot, MGG_SamplerState* state) {
    assert(device != nullptr);
    assert(slot >= 0 && slot < MAX_TEXTURE_SLOTS);

    device->samplers[slot] = state;
    device->samplerDirty |= 1 << slot;
}

void MGG_GraphicsDevice_SetIndexBuffer(MGG_GraphicsDevice* device, MGIndexElementSize size, MGG_Buffer* buffer) {
    assert(device != nullptr);
    assert(buffer != nullptr);

    device->indexBuffer = buffer;
    device->indexBufferSize = size;
}

void MGG_GraphicsDevice_SetVertexBuffer(MGG_GraphicsDevice* device, mgint slot, MGG_Buffer* buffer, mgint vertexOffset) {
    assert(device != nullptr);
    assert(buffer != nullptr);
    assert(slot >= 0 && slot < MAX_VERTEX_BUFFERS);

    device->vertexBuffers[slot] = buffer;
    device->vertexOffsets[slot] = vertexOffset;
    device->vertexBuffersDirty |= 1 << slot;
}

void MGG_GraphicsDevice_SetShader(MGG_GraphicsDevice* device, MGShaderStage stage, MGG_Shader* shader) {
    assert(device != nullptr);
    assert(shader != nullptr);
    assert(shader->stage == stage);

    device->shaders[(mgint)stage] = shader;
    device->shaderDirty = true;
}

void MGG_GraphicsDevice_SetInputLayout(MGG_GraphicsDevice* device, MGG_InputLayout* layout) {
    assert(device != nullptr);

    device->inputLayout = layout;
    device->inputLayoutDirty = true;
}

// ============================================================
// Program Cache — link vertex + fragment shaders into a program
// ============================================================

static GLuint MGL_ProgramGetOrCreate(MGG_GraphicsDevice* device, MGG_Shader* vertexShader, MGG_Shader* pixelShader) {
    assert(device != nullptr);
    assert(vertexShader != nullptr);
    assert(pixelShader != nullptr);
    assert(vertexShader->stage == MGShaderStage::Vertex);
    assert(pixelShader->stage == MGShaderStage::Pixel);

    uint64_t programId = ((uint64_t)vertexShader->id) | (((uint64_t)pixelShader->id) << 32);

    // Check the cache first
    auto it = device->programCache.find(programId);
    if (it != device->programCache.end())
        return it->second;

    // --- Create and link the program ---
    GLuint program = glCreateProgram();
    glAttachShader(program, vertexShader->shader);
    glAttachShader(program, pixelShader->shader);
    glLinkProgram(program);

    // Check link status
    GLint linkStatus = 0;
    glGetProgramiv(program, GL_LINK_STATUS, &linkStatus);
    if (linkStatus != GL_TRUE) {
        GLint logLength = 0;
        glGetProgramiv(program, GL_INFO_LOG_LENGTH, &logLength);
        if (logLength > 0) {
            std::vector<char> log(logLength);
            glGetProgramInfoLog(program, logLength, nullptr, log.data());
            fprintf(stderr, "MGL_ProgramGetOrCreate: program link failed (vs=%u, ps=%u):\n%s\n",
                    vertexShader->id, pixelShader->id, log.data());
        }
        glDeleteProgram(program);
        return 0;
    }

    // Must call glUseProgram before setting uniform values
    glUseProgram(program);

    // --- Set up uniform block bindings ---
    // For each shader (vertex + pixel), iterate the parsed bindings and
    // connect OpenGL uniform blocks to the correct UBO binding points.
    MGG_Shader* shaders[2] = { vertexShader, pixelShader };
    for (int s = 0; s < 2; s++) {
        MGG_Shader* sh = shaders[s];
        for (int i = 0; i < sh->bindingCount; i++) {
            auto& b = sh->bindings[i];
            if (b.descriptorType == MG_BINDING_TYPE_UNIFORM_BUFFER) {
                GLint numBlocks = 0;
                glGetProgramiv(program, GL_ACTIVE_UNIFORM_BLOCKS, &numBlocks);
                for (GLint bi = 0; bi < numBlocks; bi++) {
                    GLint currentBinding = -1;
                    glGetActiveUniformBlockiv(program, bi, GL_UNIFORM_BLOCK_BINDING, &currentBinding);
                    int uboBindingPoint = (int)sh->stage + (int)b.binding;
                    if (currentBinding == (GLint)b.binding) {
                        glUniformBlockBinding(program, bi, uboBindingPoint);
                    }
                }
            }
        }
    }

    // --- Set up sampler uniform → texture unit mappings ---
    // For each shader, iterate bindings that are texture/sampler types and
    // assign the corresponding sampler uniforms to texture unit = (binding - TEXTURE_SLOT_OFFSET).
    GLint numActiveUniforms = 0;
    glGetProgramiv(program, GL_ACTIVE_UNIFORMS, &numActiveUniforms);

    for (int s = 0; s < 2; s++) {
        MGG_Shader* sh = shaders[s];
        for (int i = 0; i < sh->bindingCount; i++) {
            auto& b = sh->bindings[i];
            if (b.descriptorType == MG_BINDING_TYPE_COMBINED_IMAGE_SAMPLER ||
                b.descriptorType == MG_BINDING_TYPE_SAMPLED_IMAGE ||
                b.descriptorType == MG_BINDING_TYPE_SAMPLER) {
                int textureUnit = (int)b.binding - MG_TEXTURE_SLOT_OFFSET;

                // Search active uniforms for sampler types and match by binding
                for (GLint ui = 0; ui < numActiveUniforms; ui++) {
                    char uniformName[256];
                    GLsizei nameLength = 0;
                    GLint uniformSize = 0;
                    GLenum uniformType = 0;
                    glGetActiveUniform(program, ui, sizeof(uniformName), &nameLength, &uniformSize, &uniformType, uniformName);

                    // Check if this is a sampler type
                    if (uniformType == GL_SAMPLER_2D || uniformType == GL_SAMPLER_3D ||
                        uniformType == GL_SAMPLER_CUBE || uniformType == GL_SAMPLER_2D_SHADOW ||
                        uniformType == GL_SAMPLER_2D_ARRAY ||
#if !defined(MG_EMSCRIPTEN)
                        uniformType == GL_SAMPLER_1D ||
#endif
                        uniformType == GL_INT_SAMPLER_2D || uniformType == GL_UNSIGNED_INT_SAMPLER_2D) {
                        GLint location = glGetUniformLocation(program, uniformName);
                        if (location >= 0) {
                            GLint currentUnit = -1;
                            glGetUniformiv(program, location, &currentUnit);
                            if (currentUnit == (GLint)b.binding) {
                                glUniform1i(location, textureUnit);
                            }
                        }
                    }
                }
            }
        }
    }

    // Cache the linked program
    device->programCache[programId] = program;

    GL_CHECK_ERROR();
    return program;
}

// ============================================================
// State Application Helpers (Step 8)
// ============================================================

static bool IsBlendEnabled(const MGG_BlendState_Info* info)
{
    return !(info->colorSourceBlend == MGBlend::One &&
             info->colorDestBlend == MGBlend::Zero &&
             info->alphaSourceBlend == MGBlend::One &&
             info->alphaDestBlend == MGBlend::Zero);
}

static void ApplyBlendState(MGG_GraphicsDevice* device)
{
    if (!device->blendDirty && !device->blendFactorDirty)
        return;

    if (device->blendDirty && device->blendState)
    {
        const auto& infos = device->blendState->infos;

        // Check if any render target has blending enabled
        bool anyBlendEnabled = false;
        for (int i = 0; i < MAX_RENDER_TARGETS; i++)
        {
            if (IsBlendEnabled(&infos[i]))
            {
                anyBlendEnabled = true;
                break;
            }
        }

        if (anyBlendEnabled)
        {
            glEnable(GL_BLEND);

#if !defined(MG_EMSCRIPTEN)
            // Desktop GL 4.0+ supports per-target blend
            for (int i = 0; i < MAX_RENDER_TARGETS; i++)
            {
                const auto& info = infos[i];

                if (IsBlendEnabled(&info))
                {
                    glEnablei(GL_BLEND, i);
                    glBlendFuncSeparatei(i,
                        ToGLBlendFactor(info.colorSourceBlend),
                        ToGLBlendFactor(info.colorDestBlend),
                        ToGLBlendFactor(info.alphaSourceBlend),
                        ToGLBlendFactor(info.alphaDestBlend));
                    glBlendEquationSeparatei(i,
                        ToGLBlendOp(info.colorBlendFunc),
                        ToGLBlendOp(info.alphaBlendFunc));
                }
                else
                {
                    glDisablei(GL_BLEND, i);
                }

                // Apply color write mask per target
                GLboolean r = ((int)info.colorWriteChannels & (int)MGColorWriteChannels::Red) ? GL_TRUE : GL_FALSE;
                GLboolean g = ((int)info.colorWriteChannels & (int)MGColorWriteChannels::Green) ? GL_TRUE : GL_FALSE;
                GLboolean b = ((int)info.colorWriteChannels & (int)MGColorWriteChannels::Blue) ? GL_TRUE : GL_FALSE;
                GLboolean a = ((int)info.colorWriteChannels & (int)MGColorWriteChannels::Alpha) ? GL_TRUE : GL_FALSE;
                glColorMaski(i, r, g, b, a);
            }
#else
            // WebGL2/ES 3.0: single-target blend only (use target 0)
            {
                const auto& info = infos[0];
                glBlendFuncSeparate(
                    ToGLBlendFactor(info.colorSourceBlend),
                    ToGLBlendFactor(info.colorDestBlend),
                    ToGLBlendFactor(info.alphaSourceBlend),
                    ToGLBlendFactor(info.alphaDestBlend));
                glBlendEquationSeparate(
                    ToGLBlendOp(info.colorBlendFunc),
                    ToGLBlendOp(info.alphaBlendFunc));

                GLboolean r = ((int)info.colorWriteChannels & (int)MGColorWriteChannels::Red) ? GL_TRUE : GL_FALSE;
                GLboolean g = ((int)info.colorWriteChannels & (int)MGColorWriteChannels::Green) ? GL_TRUE : GL_FALSE;
                GLboolean b = ((int)info.colorWriteChannels & (int)MGColorWriteChannels::Blue) ? GL_TRUE : GL_FALSE;
                GLboolean a = ((int)info.colorWriteChannels & (int)MGColorWriteChannels::Alpha) ? GL_TRUE : GL_FALSE;
                glColorMask(r, g, b, a);
            }
#endif
        }
        else
        {
            glDisable(GL_BLEND);

            // Still need to apply color write masks even when blend is disabled
#if !defined(MG_EMSCRIPTEN)
            for (int i = 0; i < MAX_RENDER_TARGETS; i++)
            {
                const auto& info = infos[i];
                GLboolean r = ((int)info.colorWriteChannels & (int)MGColorWriteChannels::Red) ? GL_TRUE : GL_FALSE;
                GLboolean g = ((int)info.colorWriteChannels & (int)MGColorWriteChannels::Green) ? GL_TRUE : GL_FALSE;
                GLboolean b = ((int)info.colorWriteChannels & (int)MGColorWriteChannels::Blue) ? GL_TRUE : GL_FALSE;
                GLboolean a = ((int)info.colorWriteChannels & (int)MGColorWriteChannels::Alpha) ? GL_TRUE : GL_FALSE;
                glColorMaski(i, r, g, b, a);
            }
#else
            {
                const auto& info = infos[0];
                GLboolean r = ((int)info.colorWriteChannels & (int)MGColorWriteChannels::Red) ? GL_TRUE : GL_FALSE;
                GLboolean g = ((int)info.colorWriteChannels & (int)MGColorWriteChannels::Green) ? GL_TRUE : GL_FALSE;
                GLboolean b = ((int)info.colorWriteChannels & (int)MGColorWriteChannels::Blue) ? GL_TRUE : GL_FALSE;
                GLboolean a = ((int)info.colorWriteChannels & (int)MGColorWriteChannels::Alpha) ? GL_TRUE : GL_FALSE;
                glColorMask(r, g, b, a);
            }
#endif
        }

        device->blendDirty = false;
    }

    if (device->blendFactorDirty)
    {
        glBlendColor(
            device->blendFactor[0],
            device->blendFactor[1],
            device->blendFactor[2],
            device->blendFactor[3]);
        device->blendFactorDirty = false;
    }

    GL_CHECK_ERROR();
}

static void ApplyDepthStencilState(MGG_GraphicsDevice* device)
{
    if (!device->depthStencilDirty || !device->depthStencilState)
        return;

    const auto& info = device->depthStencilState->info;

    // Depth test
    if (info.depthBufferEnable)
    {
        glEnable(GL_DEPTH_TEST);
        glDepthFunc(ToGLCompareFunc(info.depthBufferFunction));
    }
    else
    {
        glDisable(GL_DEPTH_TEST);
    }

    // Depth write
    glDepthMask(info.depthBufferWriteEnable ? GL_TRUE : GL_FALSE);

    // Stencil
    if (info.stencilEnable)
    {
        glEnable(GL_STENCIL_TEST);

        glStencilMask(info.stencilWriteMask);

        // Apply stencil function and operations (front and back faces)
        glStencilFuncSeparate(
            GL_FRONT_AND_BACK,
            ToGLCompareFunc(info.stencilFunction),
            info.referenceStencil,
            info.stencilMask);

        glStencilOpSeparate(
            GL_FRONT_AND_BACK,
            ToGLStencilOp(info.stencilFail),
            ToGLStencilOp(info.stencilDepthBufferFail),
            ToGLStencilOp(info.stencilPass));
    }
    else
    {
        glDisable(GL_STENCIL_TEST);
    }

    device->depthStencilDirty = false;
    GL_CHECK_ERROR();
}

static void ApplyRasterizerState(MGG_GraphicsDevice* device)
{
    if (!device->rasterizerDirty || !device->rasterizerState)
        return;

    const auto& info = device->rasterizerState->info;

    // Cull mode
    if (info.cullMode == MGCullMode::None)
    {
        glDisable(GL_CULL_FACE);
    }
    else
    {
        glEnable(GL_CULL_FACE);
        glCullFace(ToGLCullMode(info.cullMode));
        // SPIRV-Cross emits a Y-flip (_pos.y = -_pos.y) in all vertex shaders
        // when targeting OpenGL from HLSL→SPIR-V→GLSL. This reverses the
        // triangle winding order, so we use GL_CCW instead of GL_CW to
        // compensate and match MonoGame/DirectX CW-front-face convention.
        glFrontFace(GL_CCW);
    }

    // Fill mode (desktop only)
#if !defined(MG_EMSCRIPTEN)
    glPolygonMode(GL_FRONT_AND_BACK, ToGLFillMode(info.fillMode));
#endif

    // Scissor test
    if (info.scissorTestEnable)
        glEnable(GL_SCISSOR_TEST);
    else
        glDisable(GL_SCISSOR_TEST);

    // Depth bias / polygon offset
    if (info.depthBias != 0.0f || info.slopeScaleDepthBias != 0.0f)
    {
        glEnable(GL_POLYGON_OFFSET_FILL);
        glPolygonOffset(info.slopeScaleDepthBias, info.depthBias);
    }
    else
    {
        glDisable(GL_POLYGON_OFFSET_FILL);
    }

    // Depth clipping: GL_DEPTH_CLAMP *disables* clipping, so invert the flag
#if !defined(MG_EMSCRIPTEN)
    if (info.depthClipEnable)
        glDisable(GL_DEPTH_CLAMP);
    else
        glEnable(GL_DEPTH_CLAMP);
#endif

    // Multisample anti-aliasing
#if !defined(MG_EMSCRIPTEN)
    if (info.multiSampleAntiAlias)
        glEnable(GL_MULTISAMPLE);
    else
        glDisable(GL_MULTISAMPLE);
#endif

    device->rasterizerDirty = false;
    GL_CHECK_ERROR();
}

// ============================================================
// Index count helper — primitiveCount → index/vertex count
// ============================================================

static int MGL_GetIndexCount(MGPrimitiveType primitiveType, mgint primitiveCount)
{
    switch (primitiveType)
    {
    case MGPrimitiveType::LineList:
        return primitiveCount * 2;
    case MGPrimitiveType::LineStrip:
        return primitiveCount + 1;
    case MGPrimitiveType::TriangleList:
        return primitiveCount * 3;
    case MGPrimitiveType::TriangleStrip:
        return primitiveCount + 2;
    default:
    case MGPrimitiveType::PointList:
        return primitiveCount;
    }
}

// ============================================================
// ApplyState — deferred state application before every draw
// ============================================================

static void ApplyState(MGG_GraphicsDevice* device)
{
    // 1. Shader / program binding
    if (device->shaderDirty)
    {
        auto vs = device->shaders[(mgint)MGShaderStage::Vertex];
        auto ps = device->shaders[(mgint)MGShaderStage::Pixel];
        if (vs && ps)
        {
            GLuint program = MGL_ProgramGetOrCreate(device, vs, ps);
            if (program != device->currentProgram)
            {
                glUseProgram(program);
                device->currentProgram = program;

                // When the program changes, all resource bindings must be re-applied
                device->uniformDirty = 0xFFFFFFFF;
                device->textureDirty = 0xFFFFFFFF;
                device->samplerDirty = 0xFFFFFFFF;
            }
        }
        device->shaderDirty = false;
    }

    // 2. Blend state
    if (device->blendDirty || device->blendFactorDirty)
        ApplyBlendState(device);

    // 3. Depth/stencil state
    if (device->depthStencilDirty)
        ApplyDepthStencilState(device);

    // 4. Rasterizer state
    if (device->rasterizerDirty)
        ApplyRasterizerState(device);

    // 5. Uniform buffer bindings
    if (device->uniformDirty)
    {
        // Gather active uniform slots from both shaders
        uint32_t activeSlots = 0;
        for (int s = 0; s < (int)MGShaderStage::Count; s++)
        {
            auto sh = device->shaders[s];
            if (sh)
                activeSlots |= sh->uniformSlots;
        }

        uint32_t slotsToUpdate = device->uniformDirty & activeSlots;
        for (int slot = 0; slot < MAX_UNIFORM_BUFFER_SLOTS && slotsToUpdate; slot++)
        {
            if (slotsToUpdate & (1 << slot))
            {
                auto buffer = device->constantBuffers[slot];
                if (buffer)
                {
                    // Bind UBO to the binding point.
                    // Vertex shader uses binding point = slot,
                    // Pixel shader uses binding point = 1 + slot (offset by stage).
                    // Since we bind all active slots for both stages, we bind per-stage:
                    for (int s = 0; s < (int)MGShaderStage::Count; s++)
                    {
                        auto sh = device->shaders[s];
                        if (sh && (sh->uniformSlots & (1 << slot)))
                        {
                            int bindingPoint = s + slot;
                            glBindBufferBase(GL_UNIFORM_BUFFER, bindingPoint, buffer->handle);
                        }
                    }
                }
                slotsToUpdate &= ~(1 << slot);
            }
        }
        device->uniformDirty = 0;
    }

    // 6. Texture bindings
    if (device->textureDirty)
    {
        uint32_t activeSlots = 0;
        for (int s = 0; s < (int)MGShaderStage::Count; s++)
        {
            auto sh = device->shaders[s];
            if (sh)
                activeSlots |= sh->textureSlots;
        }

        uint32_t slotsToUpdate = device->textureDirty & activeSlots;
        for (int slot = 0; slot < MAX_TEXTURE_SLOTS && slotsToUpdate; slot++)
        {
            if (slotsToUpdate & (1 << slot))
            {
                glActiveTexture(GL_TEXTURE0 + slot);
                auto tex = device->textures[slot];
                if (tex)
                    glBindTexture(tex->target, tex->texture);
                else
                    glBindTexture(GL_TEXTURE_2D, 0);
                slotsToUpdate &= ~(1 << slot);
            }
        }
        device->textureDirty = 0;
    }

    // 7. Sampler bindings
    if (device->samplerDirty)
    {
        uint32_t activeSlots = 0;
        for (int s = 0; s < (int)MGShaderStage::Count; s++)
        {
            auto sh = device->shaders[s];
            if (sh)
                activeSlots |= sh->samplerSlots;
        }

        uint32_t slotsToUpdate = device->samplerDirty & activeSlots;
        for (int slot = 0; slot < MAX_TEXTURE_SLOTS && slotsToUpdate; slot++)
        {
            if (slotsToUpdate & (1 << slot))
            {
                auto sampler = device->samplers[slot];
                if (sampler)
                    glBindSampler(slot, sampler->sampler);
                else
                    glBindSampler(slot, 0);
                slotsToUpdate &= ~(1 << slot);
            }
        }
        device->samplerDirty = 0;
    }

    // 8. Input layout / vertex attribute setup
    if (device->inputLayoutDirty || device->vertexBuffersDirty)
    {
        auto layout = device->inputLayout;
        if (layout)
        {
            int elementCount = (int)layout->elements.size();

            // Disable all attribute slots first, then enable the ones we need
            // to avoid stale attributes from a previous layout.
            for (int i = 0; i < elementCount; i++)
                glEnableVertexAttribArray(i);

            for (int i = 0; i < elementCount; i++)
            {
                const auto& elem = layout->elements[i];
                int vbSlot = elem.VertexBufferSlot;
                auto vb = device->vertexBuffers[vbSlot];
                if (!vb)
                    continue;

                int stride = (vbSlot < (int)layout->strides.size()) ? layout->strides[vbSlot] : 0;
                auto attrib = ToGLVertexAttribType(elem.Format);

                glBindBuffer(GL_ARRAY_BUFFER, vb->handle);

                // Compute the byte offset: element's aligned offset + vertex offset * stride
                uintptr_t offset = (uintptr_t)elem.AlignedByteOffset + (uintptr_t)device->vertexOffsets[vbSlot] * stride;

                glVertexAttribPointer(
                    i,                      // location
                    attrib.size,            // component count
                    attrib.type,            // component type
                    attrib.normalized,      // normalized
                    stride,                 // stride
                    (const void*)offset     // offset
                );

                // Set up instancing divisor
                if (elem.InstanceDataStepRate > 0)
                    glVertexAttribDivisor(i, elem.InstanceDataStepRate);
                else
                    glVertexAttribDivisor(i, 0);
            }
        }
        device->inputLayoutDirty = false;
        device->vertexBuffersDirty = 0;
    }

    // 9. Index buffer binding
    if (device->indexBuffer)
    {
        glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, device->indexBuffer->handle);
    }

    GL_CHECK_ERROR();
}

// ============================================================
// Draw Calls
// ============================================================

void MGG_GraphicsDevice_Draw(MGG_GraphicsDevice* device, MGPrimitiveType primitiveType, mgint vertexStart, mgint vertexCount) {
    assert(device != nullptr);
    assert(vertexStart >= 0);

    if (vertexCount <= 0)
        return;

    ApplyState(device);

    GLenum topology = ToGLPrimitiveType(primitiveType);
    glDrawArrays(topology, vertexStart, vertexCount);
    GL_CHECK_ERROR();
}

void MGG_GraphicsDevice_DrawIndexed(MGG_GraphicsDevice* device, MGPrimitiveType primitiveType, mgint primitiveCount, mgint indexStart, mgint vertexStart) {
    assert(device != nullptr);
    assert(primitiveCount >= 0);
    assert(indexStart >= 0);
    assert(vertexStart >= 0);

    if (primitiveCount <= 0)
        return;

    ApplyState(device);

    GLenum topology = ToGLPrimitiveType(primitiveType);
    int indexCount = MGL_GetIndexCount(primitiveType, primitiveCount);
    GLenum indexType = (device->indexBufferSize == MGIndexElementSize::SixteenBits) ? GL_UNSIGNED_SHORT : GL_UNSIGNED_INT;
    int indexElementBytes = (device->indexBufferSize == MGIndexElementSize::SixteenBits) ? 2 : 4;

    // glDrawElementsBaseVertex allows a base vertex offset without modifying the index buffer
#if !defined(MG_EMSCRIPTEN)
    glDrawElementsBaseVertex(
        topology,
        indexCount,
        indexType,
        (const void*)(uintptr_t)(indexStart * indexElementBytes),
        vertexStart);
#else
    // WebGL2 does not have glDrawElementsBaseVertex; vertexStart must be 0 or
    // the caller needs to pre-offset indices.
    (void)vertexStart;
    glDrawElements(
        topology,
        indexCount,
        indexType,
        (const void*)(uintptr_t)(indexStart * indexElementBytes));
#endif
    GL_CHECK_ERROR();
}

void MGG_GraphicsDevice_DrawIndexedInstanced(MGG_GraphicsDevice* device, MGPrimitiveType primitiveType, mgint primitiveCount, mgint indexStart, mgint vertexStart, mgint instanceCount) {
    assert(device != nullptr);
    assert(primitiveCount >= 0);
    assert(indexStart >= 0);
    assert(vertexStart >= 0);
    assert(instanceCount > 0);

    if (primitiveCount <= 0)
        return;

    ApplyState(device);

    GLenum topology = ToGLPrimitiveType(primitiveType);
    int indexCount = MGL_GetIndexCount(primitiveType, primitiveCount);
    GLenum indexType = (device->indexBufferSize == MGIndexElementSize::SixteenBits) ? GL_UNSIGNED_SHORT : GL_UNSIGNED_INT;
    int indexElementBytes = (device->indexBufferSize == MGIndexElementSize::SixteenBits) ? 2 : 4;

#if !defined(MG_EMSCRIPTEN)
    glDrawElementsInstancedBaseVertex(
        topology,
        indexCount,
        indexType,
        (const void*)(uintptr_t)(indexStart * indexElementBytes),
        instanceCount,
        vertexStart);
#else
    // WebGL2 does not have glDrawElementsInstancedBaseVertex
    (void)vertexStart;
    glDrawElementsInstanced(
        topology,
        indexCount,
        indexType,
        (const void*)(uintptr_t)(indexStart * indexElementBytes),
        instanceCount);
#endif
    GL_CHECK_ERROR();
}

void MGG_GraphicsDevice_ResolveRenderTargets(MGG_GraphicsDevice* device) {
    assert(device != nullptr);

    // Generate mipmaps for any render targets that have mip levels > 1.
    // This is the OpenGL equivalent of the Vulkan blit-chain mipmap generation.
    for (int i = 0; i < device->renderTargetCount; i++)
    {
        MGG_Texture* rt = device->renderTargets[i];
        if (!rt || !rt->isRenderTarget)
            continue;

        if (rt->mipmaps > 1)
        {
            glBindTexture(rt->target, rt->texture);
            glGenerateMipmap(rt->target);
            GL_CHECK_ERROR();
        }
    }
}

void MGG_GraphicsDevice_GetBackBufferData(MGG_GraphicsDevice* device, mgint x, mgint y, mgint width, mgint height, void* data, mgint count, mgint dataBytes) {
    assert(device != nullptr);
    assert(data != nullptr);
    assert(count > 0);
    assert(dataBytes > 0);

    // Bind the default framebuffer to read from the backbuffer.
    glBindFramebuffer(GL_READ_FRAMEBUFFER, 0);
    GL_CHECK_ERROR();

    // OpenGL's origin is bottom-left, so flip the Y coordinate.
    int bbHeight = device->backbufferHeight > 0 ? device->backbufferHeight : device->viewportHeight;
    int flippedY = bbHeight - (y + height);

    // Read pixels in RGBA / unsigned byte format.
    glReadPixels(x, flippedY, width, height, GL_RGBA, GL_UNSIGNED_BYTE, data);
    GL_CHECK_ERROR();

    // OpenGL reads bottom-to-top but callers expect top-to-bottom,
    // so flip the rows in-place.
    int bytesPerPixel = 4; // GL_RGBA / GL_UNSIGNED_BYTE
    int rowBytes = width * bytesPerPixel;
    uint8_t* pixels = static_cast<uint8_t*>(data);
    std::vector<uint8_t> tempRow(rowBytes);
    for (int row = 0; row < height / 2; row++)
    {
        uint8_t* topRow = pixels + row * rowBytes;
        uint8_t* bottomRow = pixels + (height - 1 - row) * rowBytes;
        memcpy(tempRow.data(), topRow, rowBytes);
        memcpy(topRow, bottomRow, rowBytes);
        memcpy(bottomRow, tempRow.data(), rowBytes);
    }

    // Rebind the current render target if we were targeting an FBO.
    if (device->renderTargetCount > 0)
        glBindFramebuffer(GL_FRAMEBUFFER, device->fbo);
}

MGG_BlendState* MGG_BlendState_Create(MGG_GraphicsDevice* device, MGG_BlendState_Info* infos) {
    assert(device != nullptr);
    assert(infos != nullptr);

    MGG_BlendState* state = new MGG_BlendState();
    memcpy(state->infos, infos, sizeof(MGG_BlendState_Info) * MAX_RENDER_TARGETS);

    return state;
}

void MGG_BlendState_Destroy(MGG_GraphicsDevice* device, MGG_BlendState* state) {
    delete state;
}

MGG_DepthStencilState* MGG_DepthStencilState_Create(MGG_GraphicsDevice* device, MGG_DepthStencilState_Info* info) {
    MGG_DepthStencilState* state = new MGG_DepthStencilState();
    state->info = *info;
    return state;
}

void MGG_DepthStencilState_Destroy(MGG_GraphicsDevice* device, MGG_DepthStencilState* state) {
    // No OpenGL resources to clean up
    delete state;
}

MGG_RasterizerState* MGG_RasterizerState_Create(MGG_GraphicsDevice* device, MGG_RasterizerState_Info* info) {
    MGG_RasterizerState* state = new MGG_RasterizerState();
    state->info = *info;
    return state;
}

void MGG_RasterizerState_Destroy(MGG_GraphicsDevice* device, MGG_RasterizerState* state) {
    // No OpenGL resources to clean up
    delete state;
}

MGG_SamplerState* MGG_SamplerState_Create(MGG_GraphicsDevice* device, MGG_SamplerState_Info* info) {
    assert(device != nullptr);
    assert(info != nullptr);

    auto state = new MGG_SamplerState();
    state->info = *info;

    glGenSamplers(1, &state->sampler);
    GL_CHECK_ERROR();

    // Address modes
    glSamplerParameteri(state->sampler, GL_TEXTURE_WRAP_S, ToGLWrapMode(info->AddressU));
    glSamplerParameteri(state->sampler, GL_TEXTURE_WRAP_T, ToGLWrapMode(info->AddressV));
    glSamplerParameteri(state->sampler, GL_TEXTURE_WRAP_R, ToGLWrapMode(info->AddressW));

    // Min/Mag filters
    glSamplerParameteri(state->sampler, GL_TEXTURE_MIN_FILTER, ToGLMinFilter(info->Filter));
    glSamplerParameteri(state->sampler, GL_TEXTURE_MAG_FILTER, ToGLMagFilter(info->Filter));

    // Anisotropy (if supported and requested)
    if (info->Filter == MGTextureFilter::Anisotropic && info->MaximumAnisotropy > 1)
    {
        glSamplerParameterf(state->sampler, GL_TEXTURE_MAX_ANISOTROPY_EXT, static_cast<GLfloat>(info->MaximumAnisotropy));
    }

    // Mip LOD bias and clamp
    glSamplerParameterf(state->sampler, GL_TEXTURE_LOD_BIAS, info->MipMapLevelOfDetailBias);
    glSamplerParameterf(state->sampler, GL_TEXTURE_MIN_LOD, 0.0f);
    glSamplerParameterf(state->sampler, GL_TEXTURE_MAX_LOD, 1000.0f);

    // Comparison mode (for shadow maps / depth sampling)
    bool isComparison = info->FilterMode == MGTextureFilterMode::Comparison;
    if (isComparison)
    {
        glSamplerParameteri(state->sampler, GL_TEXTURE_COMPARE_MODE, GL_COMPARE_REF_TO_TEXTURE);
        glSamplerParameteri(state->sampler, GL_TEXTURE_COMPARE_FUNC, ToGLCompareFunc(info->ComparisonFunction));
    }
    else
    {
        glSamplerParameteri(state->sampler, GL_TEXTURE_COMPARE_MODE, GL_NONE);
    }

    // Border color
#if !defined(MG_EMSCRIPTEN)
    if (info->AddressU == MGTextureAddressMode::Border ||
        info->AddressV == MGTextureAddressMode::Border ||
        info->AddressW == MGTextureAddressMode::Border)
    {
        GLfloat borderColor[4];
        borderColor[0] = ((info->BorderColor >> 0) & 0xFF) / 255.0f;
        borderColor[1] = ((info->BorderColor >> 8) & 0xFF) / 255.0f;
        borderColor[2] = ((info->BorderColor >> 16) & 0xFF) / 255.0f;
        borderColor[3] = ((info->BorderColor >> 24) & 0xFF) / 255.0f;
        glSamplerParameterfv(state->sampler, GL_TEXTURE_BORDER_COLOR, borderColor);
    }
#endif

    GL_CHECK_ERROR();
    return state;
}

void MGG_SamplerState_Destroy(MGG_GraphicsDevice* device, MGG_SamplerState* state) {
    assert(device != nullptr);
    if (!state)
        return;

    if (state->sampler != 0)
    {
        glDeleteSamplers(1, &state->sampler);
        state->sampler = 0;
    }
    delete state;
}

MGG_Buffer* MGG_Buffer_Create(MGG_GraphicsDevice* device, MGBufferType type, mgint sizeInBytes) {
    assert(device != nullptr);
    assert(sizeInBytes > 0);
    if (!device || sizeInBytes <= 0) return nullptr;

    MGG_Buffer* buffer = new MGG_Buffer();
    buffer->type = type;
    buffer->target = ToGLBufferTarget(type);
    buffer->sizeInBytes = sizeInBytes;

    glGenBuffers(1, &buffer->handle);
    GL_CHECK_ERROR();

    glBindBuffer(buffer->target, buffer->handle);
    glBufferData(buffer->target, sizeInBytes, nullptr, GL_DYNAMIC_DRAW);
    glBindBuffer(buffer->target, 0);
    GL_CHECK_ERROR();

    device->all_buffers.push_back(buffer);

    return buffer;
}

void MGG_Buffer_Destroy(MGG_GraphicsDevice* device, MGG_Buffer* buffer) {
    assert(device != nullptr);
    assert(buffer != nullptr);
    if (!device || !buffer) return;

    if (buffer->handle) {
        glDeleteBuffers(1, &buffer->handle);
        buffer->handle = 0;
    }

    // Remove from tracking list
    auto it = std::find(device->all_buffers.begin(), device->all_buffers.end(), buffer);
    if (it != device->all_buffers.end())
        device->all_buffers.erase(it);

    delete buffer;
}

void MGG_Buffer_SetData(MGG_GraphicsDevice* device, MGG_Buffer*& buffer, mgint offset, mgbyte* data, mgint elementCount, mgint vertexStride, mgint elementSizeInBytes, mgbool discard) {
    assert(device != nullptr);
    assert(buffer != nullptr);
    assert(data != nullptr);
    assert(offset >= 0);
    assert(elementCount > 0);
    assert(vertexStride > 0);
    assert(elementSizeInBytes > 0);
    if (!device || !buffer || !data) return;

    auto dataSize = elementCount * vertexStride;
    if (elementSizeInBytes < vertexStride)
        dataSize -= vertexStride - elementSizeInBytes;

    // If the buffer is too small, reallocate it.
    if (offset + dataSize > buffer->sizeInBytes) {
        auto newSize = offset + dataSize;
        glBindBuffer(buffer->target, buffer->handle);
        glBufferData(buffer->target, newSize, nullptr, GL_DYNAMIC_DRAW);
        glBindBuffer(buffer->target, 0);
        buffer->sizeInBytes = newSize;
    }

    glBindBuffer(buffer->target, buffer->handle);

    if (discard) {
        // Orphan the buffer to avoid GPU stalls, then upload the data.
        glBufferData(buffer->target, buffer->sizeInBytes, nullptr, GL_DYNAMIC_DRAW);
    }

    if (elementSizeInBytes == vertexStride) {
        // Contiguous data — single upload.
        glBufferSubData(buffer->target, offset, dataSize, data);
    } else {
        // Non-contiguous data — upload element by element.
        for (mgint i = 0; i < elementCount; ++i) {
            glBufferSubData(buffer->target, offset + i * vertexStride, elementSizeInBytes, data + i * elementSizeInBytes);
        }
    }

    glBindBuffer(buffer->target, 0);
    GL_CHECK_ERROR();
}

void MGG_Buffer_GetData(MGG_GraphicsDevice* device, MGG_Buffer* buffer, mgint offset, mgbyte* data, mgint dataCount, mgint dataBytes, mgint dataStride) {
    assert(device != nullptr);
    assert(buffer != nullptr);
    assert(data != nullptr);
    assert(dataCount > 0);
    assert(dataBytes > 0);
    assert(dataStride > 0);
    if (!device || !buffer || !data) return;

    glBindBuffer(buffer->target, buffer->handle);

#if !defined(MG_EMSCRIPTEN)
    // Desktop GL — use glGetBufferSubData
    auto totalSize = dataCount * dataStride;
    if (dataStride == dataBytes) {
        glGetBufferSubData(buffer->target, offset, totalSize, data);
    } else {
        // Read into a temp buffer then scatter-copy
        std::vector<mgbyte> temp(totalSize);
        glGetBufferSubData(buffer->target, offset, totalSize, temp.data());
        for (mgint i = 0; i < dataCount; ++i) {
            memcpy(data + i * dataBytes, temp.data() + i * dataStride, dataBytes);
        }
    }
#else
    // WebGL2 / ES 3.0 — glGetBufferSubData doesn't exist, use glMapBufferRange
    auto totalSize = dataCount * dataStride;
    void* mapped = glMapBufferRange(buffer->target, offset, totalSize, GL_MAP_READ_BIT);
    if (mapped) {
        if (dataStride == dataBytes) {
            memcpy(data, mapped, totalSize);
        } else {
            auto src = static_cast<mgbyte*>(mapped);
            for (mgint i = 0; i < dataCount; ++i) {
                memcpy(data + i * dataBytes, src + i * dataStride, dataBytes);
            }
        }
        glUnmapBuffer(buffer->target);
    }
#endif

    glBindBuffer(buffer->target, 0);
    GL_CHECK_ERROR();
}

// Helper: compute mip level dimension (halved per level, minimum 1)
static mgint GetMipDimension(mgint baseSize, mgint level) {
    mgint size = baseSize >> level;
    return size > 0 ? size : 1;
}

MGG_Texture* MGG_Texture_Create(MGG_GraphicsDevice* device, MGTextureType type, MGSurfaceFormat format, mgint width, mgint height, mgint depth, mgint mipmaps, mgint slices) {
    assert(device != nullptr);
    assert(width > 0);
    assert(height > 0);
    assert(depth > 0);
    assert(mipmaps > 0);
    assert(slices > 0);
    if (!device || width <= 0 || height <= 0) return nullptr;

    MGG_Texture* texture = new MGG_Texture();
    texture->type = type;
    texture->format = format;
    texture->target = ToGLTextureTarget(type);
    texture->width = width;
    texture->height = height;
    texture->depth = depth;
    texture->mipmaps = mipmaps;
    texture->slices = slices;
    texture->isRenderTarget = false;

    GLenum internalFormat = ToGLInternalFormat(format);

    glGenTextures(1, &texture->texture);
    GL_CHECK_ERROR();
    glBindTexture(texture->target, texture->texture);
    GL_CHECK_ERROR();

    switch (type) {
    case MGTextureType::_2D:
        glTexStorage2D(GL_TEXTURE_2D, mipmaps, internalFormat, width, height);
        GL_CHECK_ERROR();
        break;
    case MGTextureType::_3D:
        glTexStorage3D(GL_TEXTURE_3D, mipmaps, internalFormat, width, height, depth);
        GL_CHECK_ERROR();
        break;
    case MGTextureType::Cube:
        // Cube map storage: glTexStorage2D with GL_TEXTURE_CUBE_MAP allocates all 6 faces
        glTexStorage2D(GL_TEXTURE_CUBE_MAP, mipmaps, internalFormat, width, height);
        GL_CHECK_ERROR();
        break;
    default:
        assert(!"Unsupported texture type in MGG_Texture_Create!");
        break;
    }

    // Set default sampling parameters
    glTexParameteri(texture->target, GL_TEXTURE_MIN_FILTER, mipmaps > 1 ? GL_LINEAR_MIPMAP_LINEAR : GL_LINEAR);
    glTexParameteri(texture->target, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
    glTexParameteri(texture->target, GL_TEXTURE_WRAP_S, GL_REPEAT);
    glTexParameteri(texture->target, GL_TEXTURE_WRAP_T, GL_REPEAT);
    if (type == MGTextureType::_3D || type == MGTextureType::Cube)
        glTexParameteri(texture->target, GL_TEXTURE_WRAP_R, GL_REPEAT);

    glBindTexture(texture->target, 0);
    GL_CHECK_ERROR();

    device->all_textures.push_back(texture);

    return texture;
}

MGG_Texture* MGG_RenderTarget_Create(MGG_GraphicsDevice* device, MGTextureType type, MGSurfaceFormat format, mgint width, mgint height, mgint depth, mgint mipmaps, mgint slices, MGDepthFormat depthFormat, mgint multiSampleCount, MGRenderTargetUsage usage) {
    assert(device != nullptr);
    assert(width > 0);
    assert(height > 0);
    if (!device || width <= 0 || height <= 0) return nullptr;

    // Create the color texture via normal texture creation path
    MGG_Texture* texture = MGG_Texture_Create(device, type, format, width, height, depth, mipmaps, slices);
    if (!texture) return nullptr;

    texture->isRenderTarget = true;
    texture->depthFormat = depthFormat;
    texture->multiSampleCount = multiSampleCount;
    texture->usage = usage;

    // Create depth renderbuffer if requested
    if (depthFormat != MGDepthFormat::None) {
        GLenum glDepthFormat = ToGLDepthFormat(depthFormat);
        glGenRenderbuffers(1, &texture->depthRenderbuffer);
        GL_CHECK_ERROR();
        glBindRenderbuffer(GL_RENDERBUFFER, texture->depthRenderbuffer);
        GL_CHECK_ERROR();
        glRenderbufferStorage(GL_RENDERBUFFER, glDepthFormat, width, height);
        GL_CHECK_ERROR();
        glBindRenderbuffer(GL_RENDERBUFFER, 0);
        GL_CHECK_ERROR();
    }

    return texture;
}

void MGG_Texture_Destroy(MGG_GraphicsDevice* device, MGG_Texture* texture) {
    assert(device != nullptr);
    if (!device || !texture) return;

    // Delete depth renderbuffer if present
    if (texture->depthRenderbuffer) {
        glDeleteRenderbuffers(1, &texture->depthRenderbuffer);
        texture->depthRenderbuffer = 0;
    }

    // Delete the texture object
    if (texture->texture) {
        glDeleteTextures(1, &texture->texture);
        texture->texture = 0;
    }
    GL_CHECK_ERROR();

    // Remove from tracking list
    auto it = std::find(device->all_textures.begin(), device->all_textures.end(), texture);
    if (it != device->all_textures.end())
        device->all_textures.erase(it);

    delete texture;
}

void MGG_Texture_SetData(MGG_GraphicsDevice* device, MGG_Texture* texture, mgint level, mgint slice, mgint x, mgint y, mgint z, mgint width, mgint height, mgint depth, mgbyte* data, mgint dataBytes) {
    assert(device != nullptr);
    assert(texture != nullptr);
    assert(data != nullptr);
    assert(dataBytes > 0);
    if (!device || !texture || !data) return;

    // Clamp width/height to mip level dimensions if zero
    mgint mipWidth = GetMipDimension(texture->width, level);
    mgint mipHeight = GetMipDimension(texture->height, level);
    mgint mipDepth = GetMipDimension(texture->depth, level);
    if (width == 0 && height == 0) {
        width = mipWidth;
        height = mipHeight;
    }
    if (texture->type == MGTextureType::_2D || texture->type == MGTextureType::Cube) {
        depth = 1;
        z = 0;
    } else if (depth == 0) {
        depth = mipDepth;
    }

    bool compressed = IsCompressedFormat(texture->format);

    switch (texture->type) {
    case MGTextureType::_2D:
        glBindTexture(GL_TEXTURE_2D, texture->texture);
        if (compressed) {
            glCompressedTexSubImage2D(GL_TEXTURE_2D, level, x, y, width, height,
                ToGLInternalFormat(texture->format), dataBytes, data);
        } else {
            glTexSubImage2D(GL_TEXTURE_2D, level, x, y, width, height,
                ToGLFormat(texture->format), ToGLType(texture->format), data);
        }
        glBindTexture(GL_TEXTURE_2D, 0);
        break;

    case MGTextureType::Cube: {
        // For cube maps, slice selects the face: 0-5 => GL_TEXTURE_CUBE_MAP_POSITIVE_X + slice
        GLenum faceTarget = GL_TEXTURE_CUBE_MAP_POSITIVE_X + slice;
        glBindTexture(GL_TEXTURE_CUBE_MAP, texture->texture);
        if (compressed) {
            glCompressedTexSubImage2D(faceTarget, level, x, y, width, height,
                ToGLInternalFormat(texture->format), dataBytes, data);
        } else {
            glTexSubImage2D(faceTarget, level, x, y, width, height,
                ToGLFormat(texture->format), ToGLType(texture->format), data);
        }
        glBindTexture(GL_TEXTURE_CUBE_MAP, 0);
        break;
    }

    case MGTextureType::_3D:
        glBindTexture(GL_TEXTURE_3D, texture->texture);
        if (compressed) {
            glCompressedTexSubImage3D(GL_TEXTURE_3D, level, x, y, z, width, height, depth,
                ToGLInternalFormat(texture->format), dataBytes, data);
        } else {
            glTexSubImage3D(GL_TEXTURE_3D, level, x, y, z, width, height, depth,
                ToGLFormat(texture->format), ToGLType(texture->format), data);
        }
        glBindTexture(GL_TEXTURE_3D, 0);
        break;

    default:
        assert(!"Unsupported texture type in MGG_Texture_SetData!");
        break;
    }
    GL_CHECK_ERROR();
}

void MGG_Texture_GetData(MGG_GraphicsDevice* device, MGG_Texture* texture, mgint level, mgint slice, mgint x, mgint y, mgint z, mgint width, mgint height, mgint depth, mgbyte* data, mgint dataBytes) {
    assert(device != nullptr);
    assert(texture != nullptr);
    assert(data != nullptr);
    assert(dataBytes > 0);
    if (!device || !texture || !data) return;

    // Clamp width/height to mip level dimensions if zero
    mgint mipWidth = GetMipDimension(texture->width, level);
    mgint mipHeight = GetMipDimension(texture->height, level);
    mgint mipDepth = GetMipDimension(texture->depth, level);
    if (width == 0 && height == 0) {
        width = mipWidth;
        height = mipHeight;
    }
    if (texture->type == MGTextureType::_2D || texture->type == MGTextureType::Cube) {
        depth = 1;
        z = 0;
    } else if (depth == 0) {
        depth = mipDepth;
    }

#if !defined(MG_EMSCRIPTEN)
    // Desktop GL — use glGetTexImage for full mip level reads,
    // or FBO + glReadPixels for sub-region reads.
    if (x == 0 && y == 0 && width == mipWidth && height == mipHeight && !IsCompressedFormat(texture->format)) {
        // Full mip level read — use glGetTexImage
        GLenum bindTarget = texture->target;
        if (texture->type == MGTextureType::Cube)
            bindTarget = GL_TEXTURE_CUBE_MAP_POSITIVE_X + slice;

        glBindTexture(texture->target, texture->texture);
        if (texture->type == MGTextureType::Cube) {
            glGetTexImage(GL_TEXTURE_CUBE_MAP_POSITIVE_X + slice, level,
                ToGLFormat(texture->format), ToGLType(texture->format), data);
        } else {
            glGetTexImage(texture->target, level,
                ToGLFormat(texture->format), ToGLType(texture->format), data);
        }
        glBindTexture(texture->target, 0);
    } else if (IsCompressedFormat(texture->format)) {
        // Compressed format — use glGetCompressedTexImage
        glBindTexture(texture->target, texture->texture);
        if (texture->type == MGTextureType::Cube) {
            glGetCompressedTexImage(GL_TEXTURE_CUBE_MAP_POSITIVE_X + slice, level, data);
        } else {
            glGetCompressedTexImage(texture->target, level, data);
        }
        glBindTexture(texture->target, 0);
    } else {
        // Sub-region read — attach to a temporary FBO and use glReadPixels
        GLuint tempFBO;
        glGenFramebuffers(1, &tempFBO);
        glBindFramebuffer(GL_FRAMEBUFFER, tempFBO);

        if (texture->type == MGTextureType::Cube) {
            glFramebufferTexture2D(GL_FRAMEBUFFER, GL_COLOR_ATTACHMENT0,
                GL_TEXTURE_CUBE_MAP_POSITIVE_X + slice, texture->texture, level);
        } else if (texture->type == MGTextureType::_3D) {
            glFramebufferTextureLayer(GL_FRAMEBUFFER, GL_COLOR_ATTACHMENT0,
                texture->texture, level, z);
        } else {
            glFramebufferTexture2D(GL_FRAMEBUFFER, GL_COLOR_ATTACHMENT0,
                GL_TEXTURE_2D, texture->texture, level);
        }

        assert(glCheckFramebufferStatus(GL_FRAMEBUFFER) == GL_FRAMEBUFFER_COMPLETE);
        glReadPixels(x, y, width, height,
            ToGLFormat(texture->format), ToGLType(texture->format), data);

        glBindFramebuffer(GL_FRAMEBUFFER, 0);
        glDeleteFramebuffers(1, &tempFBO);
    }
#else
    // WebGL2 / ES 3.0 — no glGetTexImage; always use FBO + glReadPixels
    GLuint tempFBO;
    glGenFramebuffers(1, &tempFBO);
    glBindFramebuffer(GL_FRAMEBUFFER, tempFBO);

    if (texture->type == MGTextureType::Cube) {
        glFramebufferTexture2D(GL_FRAMEBUFFER, GL_COLOR_ATTACHMENT0,
            GL_TEXTURE_CUBE_MAP_POSITIVE_X + slice, texture->texture, level);
    } else if (texture->type == MGTextureType::_3D) {
        glFramebufferTextureLayer(GL_FRAMEBUFFER, GL_COLOR_ATTACHMENT0,
            texture->texture, level, z);
    } else {
        glFramebufferTexture2D(GL_FRAMEBUFFER, GL_COLOR_ATTACHMENT0,
            GL_TEXTURE_2D, texture->texture, level);
    }

    assert(glCheckFramebufferStatus(GL_FRAMEBUFFER) == GL_FRAMEBUFFER_COMPLETE);
    glReadPixels(x, y, width, height,
        ToGLFormat(texture->format), ToGLType(texture->format), data);

    glBindFramebuffer(GL_FRAMEBUFFER, 0);
    glDeleteFramebuffers(1, &tempFBO);
#endif
    GL_CHECK_ERROR();
}

MGG_InputLayout* MGG_InputLayout_Create(MGG_GraphicsDevice* device, MGG_Shader* vertexShader, mgint* strides, mgint streamCount, MGG_InputElement* elements, mgint elementCount) {
    assert(device != nullptr);
    assert(streamCount >= 0);
    assert(strides != nullptr);
    assert(elements != nullptr);
    assert(elementCount >= 0);

    auto layout = new MGG_InputLayout();

    // Copy the per-stream strides
    layout->strides.resize(streamCount);
    for (int i = 0; i < streamCount; i++)
        layout->strides[i] = strides[i];

    // Copy the vertex element descriptions
    layout->elements.resize(elementCount);
    for (int i = 0; i < elementCount; i++)
        layout->elements[i] = elements[i];

    return layout;
}

void MGG_InputLayout_Destroy(MGG_GraphicsDevice* device, MGG_InputLayout* layout) {
    assert(device != nullptr);
    assert(layout != nullptr);

    if (layout == nullptr)
        return;

    delete layout;
}

MGG_Shader* MGG_Shader_Create(MGG_GraphicsDevice* device, MGShaderStage stage, mgbyte* bytecode, mgint sizeInBytes) {
    assert(device != nullptr);
    assert(bytecode != nullptr);
    assert(sizeInBytes > 0);

    MGG_Shader* shader = new MGG_Shader();
    shader->stage = stage;

    // --- Parse the bytecode container header ---
    shader->uniformCount = *(mgint*)bytecode;  bytecode += sizeof(mgint);  sizeInBytes -= sizeof(mgint);
    shader->uniformSlots = *(mguint*)bytecode; bytecode += sizeof(mguint); sizeInBytes -= sizeof(mguint);
    shader->textureSlots = *(mguint*)bytecode; bytecode += sizeof(mguint); sizeInBytes -= sizeof(mguint);
    shader->samplerSlots = *(mguint*)bytecode; bytecode += sizeof(mguint); sizeInBytes -= sizeof(mguint);

    shader->bindingCount = *(mgint*)bytecode;  bytecode += sizeof(mgint);  sizeInBytes -= sizeof(mgint);

    // Read the binding entries (each is 24 bytes, matching the content pipeline's binary layout)
    shader->bindings.resize(shader->bindingCount);
    if (shader->bindingCount > 0) {
        size_t bindingsSize = sizeof(MGShaderBinding) * shader->bindingCount;
        memcpy(shader->bindings.data(), bytecode, bindingsSize);
        bytecode += bindingsSize;
        sizeInBytes -= bindingsSize;
    }

    // --- The remaining bytes are GLSL source text ---
    // Store original bytecode for reference (the GLSL part)
    shader->bytecode.assign(bytecode, bytecode + sizeInBytes);

    // The GLSL source has binding qualifiers already stripped at compile time
    // by the content pipeline (ShaderProfile.OpenGL4.cs).
    const char* glslSource = (const char*)bytecode;
    GLint glslLength = (GLint)sizeInBytes;

    // --- Create and compile the GL shader ---
    GLenum glStage = (stage == MGShaderStage::Vertex) ? GL_VERTEX_SHADER : GL_FRAGMENT_SHADER;
    shader->shader = glCreateShader(glStage);
    glShaderSource(shader->shader, 1, &glslSource, &glslLength);
    glCompileShader(shader->shader);

    // Check compilation status
    GLint compileStatus = 0;
    glGetShaderiv(shader->shader, GL_COMPILE_STATUS, &compileStatus);
    if (compileStatus != GL_TRUE) {
        GLint logLength = 0;
        glGetShaderiv(shader->shader, GL_INFO_LOG_LENGTH, &logLength);
        if (logLength > 0) {
            std::vector<char> log(logLength);
            glGetShaderInfoLog(shader->shader, logLength, nullptr, log.data());
            fprintf(stderr, "MGG_Shader_Create: %s shader compilation failed:\n%s\n",
                    (stage == MGShaderStage::Vertex) ? "Vertex" : "Fragment", log.data());
        }
        glDeleteShader(shader->shader);
        delete shader;
        return nullptr;
    }

    shader->id = ++device->currentShaderId;
    device->all_shaders.push_back(shader);

    GL_CHECK_ERROR();
    return shader;
}

void MGG_Shader_Destroy(MGG_GraphicsDevice* device, MGG_Shader* shader) {
    assert(device != nullptr);
    assert(shader != nullptr);

    if (!shader)
        return;

    // Remove any cached programs that reference this shader
    auto it = device->programCache.begin();
    while (it != device->programCache.end()) {
        uint64_t key = it->first;
        uint32_t vsId = (uint32_t)(key & 0xFFFFFFFF);
        uint32_t psId = (uint32_t)(key >> 32);
        if (vsId == shader->id || psId == shader->id) {
            glDeleteProgram(it->second);
            if (device->currentProgram == it->second)
                device->currentProgram = 0;
            it = device->programCache.erase(it);
        } else {
            ++it;
        }
    }

    // Delete the GL shader object
    if (shader->shader) {
        glDeleteShader(shader->shader);
        shader->shader = 0;
    }

    mg_remove(device->all_shaders, shader);
    delete shader;
}

MGG_OcclusionQuery* MGG_OcclusionQuery_Create(MGG_GraphicsDevice* device) {
    assert(device != nullptr);
    if (!device) return nullptr;

    auto query = new MGG_OcclusionQuery();
    glGenQueries(1, &query->query);
    GL_CHECK_ERROR();

    return query;
}

void MGG_OcclusionQuery_Destroy(MGG_GraphicsDevice* device, MGG_OcclusionQuery* query) {
    assert(device != nullptr);
    assert(query != nullptr);
    if (!device || !query) return;

    if (query->query != 0)
    {
        glDeleteQueries(1, &query->query);
        GL_CHECK_ERROR();
    }

    delete query;
}

void MGG_OcclusionQuery_Begin(MGG_GraphicsDevice* device, MGG_OcclusionQuery* query) {
    assert(device != nullptr);
    assert(query != nullptr);
    if (!device || !query) return;

#if defined(MG_EMSCRIPTEN)
    glBeginQuery(GL_ANY_SAMPLES_PASSED, query->query);
#else
    glBeginQuery(GL_SAMPLES_PASSED, query->query);
#endif
    GL_CHECK_ERROR();

    query->isActive = true;
    query->isComplete = false;
    query->pixelCount = 0;
}

void MGG_OcclusionQuery_End(MGG_GraphicsDevice* device, MGG_OcclusionQuery* query) {
    assert(device != nullptr);
    assert(query != nullptr);
    if (!device || !query || !query->isActive) return;

#if defined(MG_EMSCRIPTEN)
    glEndQuery(GL_ANY_SAMPLES_PASSED);
#else
    glEndQuery(GL_SAMPLES_PASSED);
#endif
    GL_CHECK_ERROR();

    query->isActive = false;
}

mgbyte MGG_OcclusionQuery_GetResult(MGG_GraphicsDevice* device, MGG_OcclusionQuery* query, mgint& pixelCount) {
    assert(device != nullptr);
    assert(query != nullptr);
    if (!device || !query) {
        pixelCount = 0;
        return false;
    }

    // If the result was already retrieved, return it immediately.
    if (query->isComplete)
    {
        pixelCount = query->pixelCount;
        return true;
    }

    // Poll for result availability without stalling.
    GLuint available = 0;
    glGetQueryObjectuiv(query->query, GL_QUERY_RESULT_AVAILABLE, &available);
    GL_CHECK_ERROR();

    if (available)
    {
        GLuint result = 0;
        glGetQueryObjectuiv(query->query, GL_QUERY_RESULT, &result);
        GL_CHECK_ERROR();

        query->pixelCount = (mgint)result;
        query->isComplete = true;
        pixelCount = query->pixelCount;
        return true;
    }

    // Not ready yet.
    pixelCount = 0;
    return false;
}
