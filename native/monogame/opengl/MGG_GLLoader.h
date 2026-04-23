// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

// GL function loader — dynamically loads OpenGL 1.2+ functions via
// SDL_GL_GetProcAddress on all desktop platforms.
// Emscripten/WASM uses <GLES3/gl3.h> directly and does not need this.
//
// NOTE: This header is intended to be included ONLY by MGG_OpenGL.cpp.
// It defines macros that redirect GL function names to function pointers.
// Include it AFTER all SDL/GL header includes.

#ifndef MGG_GLLOADER_H
#define MGG_GLLOADER_H

#if !defined(MG_EMSCRIPTEN)

#include <SDL.h>
#include <SDL_opengl.h>
#include <SDL_opengl_glext.h>

// Declare prefixed function pointer variables.
// Using mgl_ prefix avoids symbol conflicts with platform GL libraries
// (e.g., macOS OpenGL.framework declares these as functions in its headers).
#define MGL_GL_FUNC(type, name) extern type mgl_##name;
#include "MGG_GLFunctions.inc"
#undef MGL_GL_FUNC

// Load all GL function pointers via SDL_GL_GetProcAddress.
// Must be called AFTER SDL_GL_MakeCurrent succeeds.
// Returns false if any required function could not be loaded.
bool MGL_LoadGLFunctions();

// Redirect GL function names to our prefixed function pointers.
// This allows all existing calling code to work unchanged.

// GL 1.2
#define glBlendColor mgl_glBlendColor
#define glTexSubImage3D mgl_glTexSubImage3D

// GL 1.3
#define glActiveTexture mgl_glActiveTexture
#define glCompressedTexSubImage2D mgl_glCompressedTexSubImage2D
#define glCompressedTexSubImage3D mgl_glCompressedTexSubImage3D
#define glGetCompressedTexImage mgl_glGetCompressedTexImage

// GL 1.4
#define glBlendFuncSeparate mgl_glBlendFuncSeparate

// GL 1.5
#define glBeginQuery mgl_glBeginQuery
#define glBindBuffer mgl_glBindBuffer
#define glBufferData mgl_glBufferData
#define glBufferSubData mgl_glBufferSubData
#define glDeleteBuffers mgl_glDeleteBuffers
#define glDeleteQueries mgl_glDeleteQueries
#define glEndQuery mgl_glEndQuery
#define glGenBuffers mgl_glGenBuffers
#define glGenQueries mgl_glGenQueries
#define glGetBufferSubData mgl_glGetBufferSubData
#define glGetQueryObjectuiv mgl_glGetQueryObjectuiv
#define glUnmapBuffer mgl_glUnmapBuffer

// GL 2.0
#define glAttachShader mgl_glAttachShader
#define glBlendEquationSeparate mgl_glBlendEquationSeparate
#define glCompileShader mgl_glCompileShader
#define glCreateProgram mgl_glCreateProgram
#define glCreateShader mgl_glCreateShader
#define glDeleteProgram mgl_glDeleteProgram
#define glDeleteShader mgl_glDeleteShader
#define glDisableVertexAttribArray mgl_glDisableVertexAttribArray
#define glDrawBuffers mgl_glDrawBuffers
#define glEnableVertexAttribArray mgl_glEnableVertexAttribArray
#define glGetActiveUniform mgl_glGetActiveUniform
#define glGetProgramInfoLog mgl_glGetProgramInfoLog
#define glGetProgramiv mgl_glGetProgramiv
#define glGetShaderInfoLog mgl_glGetShaderInfoLog
#define glGetShaderiv mgl_glGetShaderiv
#define glGetUniformiv mgl_glGetUniformiv
#define glGetUniformLocation mgl_glGetUniformLocation
#define glLinkProgram mgl_glLinkProgram
#define glShaderSource mgl_glShaderSource
#define glStencilFuncSeparate mgl_glStencilFuncSeparate
#define glStencilOpSeparate mgl_glStencilOpSeparate
#define glUniform1i mgl_glUniform1i
#define glUniform4fv mgl_glUniform4fv
#define glUseProgram mgl_glUseProgram
#define glVertexAttribPointer mgl_glVertexAttribPointer

// GL 3.0
#define glBindBufferBase mgl_glBindBufferBase
#define glBindFramebuffer mgl_glBindFramebuffer
#define glBindRenderbuffer mgl_glBindRenderbuffer
#define glBindVertexArray mgl_glBindVertexArray
#define glCheckFramebufferStatus mgl_glCheckFramebufferStatus
#define glColorMaski mgl_glColorMaski
#define glDeleteFramebuffers mgl_glDeleteFramebuffers
#define glDeleteRenderbuffers mgl_glDeleteRenderbuffers
#define glDeleteVertexArrays mgl_glDeleteVertexArrays
#define glDisablei mgl_glDisablei
#define glEnablei mgl_glEnablei
#define glFramebufferRenderbuffer mgl_glFramebufferRenderbuffer
#define glFramebufferTexture2D mgl_glFramebufferTexture2D
#define glFramebufferTextureLayer mgl_glFramebufferTextureLayer
#define glGenerateMipmap mgl_glGenerateMipmap
#define glGenFramebuffers mgl_glGenFramebuffers
#define glGenRenderbuffers mgl_glGenRenderbuffers
#define glGenVertexArrays mgl_glGenVertexArrays
#define glMapBufferRange mgl_glMapBufferRange
#define glRenderbufferStorage mgl_glRenderbufferStorage

// GL 3.1
#define glGetUniformBlockIndex mgl_glGetUniformBlockIndex
#define glUniformBlockBinding mgl_glUniformBlockBinding

// GL 3.2
#define glDrawElementsBaseVertex mgl_glDrawElementsBaseVertex
#define glDrawElementsInstancedBaseVertex mgl_glDrawElementsInstancedBaseVertex

// GL 3.3
#define glBindSampler mgl_glBindSampler
#define glDeleteSamplers mgl_glDeleteSamplers
#define glGenSamplers mgl_glGenSamplers
#define glSamplerParameterf mgl_glSamplerParameterf
#define glSamplerParameterfv mgl_glSamplerParameterfv
#define glSamplerParameteri mgl_glSamplerParameteri
#define glVertexAttribDivisor mgl_glVertexAttribDivisor

// GL 4.0
#define glBlendEquationSeparatei mgl_glBlendEquationSeparatei
#define glBlendFuncSeparatei mgl_glBlendFuncSeparatei

// GL 4.2 (ARB_texture_storage)
#define glTexStorage2D mgl_glTexStorage2D
#define glTexStorage3D mgl_glTexStorage3D

#endif // !defined(MG_EMSCRIPTEN)
#endif // MGG_GLLOADER_H
