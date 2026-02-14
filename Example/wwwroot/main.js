// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

import { dotnet } from './_framework/dotnet.js'

const { setModuleImports, getAssemblyExports, getConfig } = await dotnet
    .withDiagnosticTracing(true)
    .withApplicationArgumentsFromQuery()
    .create();

setModuleImports('main.js', {
    window: {
        location: {
            href: () => globalThis.window.location.href
        }
    }
});

var canvas = document.getElementById("canvas");
dotnet.instance.Module.canvas = canvas;

// Populate the emscripten virtual filesystem with game content
const FS = dotnet.instance.Module.FS;
FS.mkdir('/Content');

const contentFiles = [
    'test.xnb',
    'testsound.xnb'
];

// Log progress to console (don't use canvas 2D context - it would prevent WebGL)
const logProgress = (loaded, total, currentFile) => {
    if (currentFile) {
        console.log(`Loading: ${currentFile} (${loaded}/${total})`);
    } else {
        console.log(`Content loading progress: ${loaded}/${total}`);
    }
};

logProgress(0, contentFiles.length, '');

let loadedCount = 0;
for (const file of contentFiles) {
    logProgress(loadedCount, contentFiles.length, file);
    const resp = await fetch(`Content/${file}`);
    if (!resp.ok) {
        console.error(`Failed to fetch Content/${file}: ${resp.status}`);
        loadedCount++;
        continue;
    }
    const data = new Uint8Array(await resp.arrayBuffer());
    FS.writeFile(`/Content/${file}`, data);
    console.log(`Loaded Content/${file} into VFS (${data.length} bytes)`);
    loadedCount++;
}

console.log('Content loading complete, starting game...');

await dotnet.run();