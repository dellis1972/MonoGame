// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

// GL function loader implementation — loads OpenGL 1.2+ function pointers
// via SDL_GL_GetProcAddress. Must be called after SDL_GL_MakeCurrent.
//
// NOTE: We intentionally do NOT include MGG_GLLoader.h here to avoid
// the #define macros redirecting our mgl_ symbol definitions.

#include <SDL.h>
#include <SDL_opengl.h>
#include <SDL_opengl_glext.h>

#include <cstdio>

#if !defined(MG_EMSCRIPTEN)

// Define all function pointer variables, initialized to nullptr.
#define MGL_GL_FUNC(type, name) type mgl_##name = nullptr;
#include "MGG_GLFunctions.inc"
#undef MGL_GL_FUNC

bool MGL_LoadGLFunctions()
{
    bool success = true;

#define MGL_GL_FUNC(type, name) \
    mgl_##name = (type)SDL_GL_GetProcAddress(#name); \
    if (!mgl_##name) { \
        fprintf(stderr, "Failed to load GL function: %s\n", #name); \
        success = false; \
    }
#include "MGG_GLFunctions.inc"
#undef MGL_GL_FUNC

    return success;
}

#endif // !defined(MG_EMSCRIPTEN)
