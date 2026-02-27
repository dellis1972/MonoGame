// MonoGame - Copyright (C) MonoGame Foundation, Inc
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

#include "mg_common.h"
#include "api_MG_Asset.h"
#include <stdio.h>

#if defined(MG_EMSCRIPTEN)
#include <dirent.h>
#include <sys/stat.h>
#include <emscripten.h>
#endif

struct MG_Asset
{
    FILE* file;
};

#if defined(MG_EMSCRIPTEN)
void listVirtualFileSystem(const char* path, int indent = 0) {
    DIR* dir = opendir(path);
    if (!dir) {
        printf("%*sFailed to open directory: %s\n", indent * 2, "", path);
        return;
    }

    struct dirent* entry;
    while ((entry = readdir(dir)) != nullptr) {
        // Skip "." and ".." to avoid infinite recursion
        if (strcmp(entry->d_name, ".") == 0 || strcmp(entry->d_name, "..") == 0)
            continue;

        // Build full path
        char fullPath[1024];
        snprintf(fullPath, sizeof(fullPath), "%s/%s", path, entry->d_name);

        struct stat st;
        if (stat(fullPath, &st) == 0 && S_ISDIR(st.st_mode)) {
            printf("%*s[DIR]  %s\n", indent * 2, "", entry->d_name);
            listVirtualFileSystem(fullPath, indent + 1);
        } else {
            printf("%*s[FILE] %s\n", indent * 2, "", entry->d_name);
        }
    }

    closedir(dir);
}
#endif

mgbool MG_Asset_Open(const char* path, MG_Asset*& handle, mglong& length)
{
    handle = new MG_Asset();
    printf("Trying to open file: %s\n", path);
#if defined(MG_EMSCRIPTEN)
    // Emscripten's virtual file system can sometimes have issues with paths that don't start with a leading slash, so we add one if it's not already present.

    // lets see what is in the virtual file system for debugging purposes
    printf("Listing files in virtual file system:\n");
    listVirtualFileSystem("/"); // Start listing from the root of the virtual file system

    // Append a leading / to the path to ensure it is treated as a file and not a directory, which can cause issues in some environments
    char modifiedPath[1024];
    snprintf(modifiedPath, sizeof(modifiedPath), "/%s", path);
    path = modifiedPath;
#endif
    handle->file = fopen(path, "rb");
    if (handle->file == nullptr)
    {
        printf("Failed to open file: %s\n", path);
        delete handle;
        return false;
    }

    if (fseek(handle->file, 0, SEEK_END) != 0)
    {
        printf("Failed to seek to end of file: %s\n", path);
        //unable to seek file for some reason
        delete handle;
        return false;
    }

    length = ftell(handle->file);

    if (fseek(handle->file, 0, SEEK_SET) != 0)
    {
        printf("Failed to seek back to start of file: %s\n", path);
        //unable to seek back to file start for some reason
        delete handle;
        return false;
    }

    printf("Successfully opened file: %s (length: %lld bytes)\n", path, length);

    return true;
}

mgint MG_Asset_Read(MG_Asset* handle, mgbyte* buffer, mglong count)
{
    return fread(buffer, 1, count, handle->file);
}

mglong MG_Asset_Seek(MG_Asset* handle, mglong offset, mgint whence)
{
    int origin;
    switch (whence)
    {
        default:
        case 0: // Begin
            origin = SEEK_SET;
            break;
        case 1: // Current
            origin = SEEK_CUR;
            break;
        case 2: // End
            origin = SEEK_END;
            break;
    }

    fseek(handle->file, offset, origin);

    return ftell(handle->file);
}

void MG_Asset_Close(MG_Asset* handle)
{
    fclose(handle->file);
    delete handle;
}
