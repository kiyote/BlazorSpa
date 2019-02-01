const filesToCache = [
    // Application
    '/_framework/_bin/Blazor.Extensions.Logging.dll',
    '/_framework/_bin/Blazor.Extensions.Logging.JS.dll',
    '/_framework/_bin/BlazorSignalR.dll',
    '/_framework/_bin/BlazorSignalR.JS.dll',
    '/_framework/_bin/BlazorSpa.Client.dll',
    '/_framework/_bin/BlazorSpa.Model.dll',
    '/_framework/_bin/Cloudcrate.AspNetCore.Blazor.Browser.Storage.dll',
    '/_framework/_bin/Microsoft.AspNetCore.Blazor.Browser.dll',
    '/_framework/_bin/Microsoft.AspNetCore.Blazor.dll',
    '/_framework/_bin/Microsoft.AspNetCore.Blazor.TagHelperWorkaround.dll',
    '/_framework/_bin/Microsoft.AspNetCore.Connections.Abstractions.dll',
    '/_framework/_bin/Microsoft.AspNetCore.Http.Connections.Client.dll',
    '/_framework/_bin/Microsoft.AspNetCore.Http.Connections.Common.dll',
    '/_framework/_bin/Microsoft.AspNetCore.Http.Features.dll',
    '/_framework/_bin/Microsoft.AspNetCore.SignalR.Client.Core.dll',
    '/_framework/_bin/Microsoft.AspNetCore.SignalR.Client.dll',
    '/_framework/_bin/Microsoft.AspNetCore.SignalR.Common.dll',
    '/_framework/_bin/Microsoft.AspNetCore.SignalR.Protocols.Json.dll',
    '/_framework/_bin/Microsoft.AspNetCore.WebUtilities.dll',
    '/_framework/_bin/Microsoft.Extensions.Configuration.Abstractions.dll',
    '/_framework/_bin/Microsoft.Extensions.Configuration.Binder.dll',
    '/_framework/_bin/Microsoft.Extensions.Configuration.dll',
    '/_framework/_bin/Microsoft.Extensions.DependencyInjection.Abstractions.dll',
    '/_framework/_bin/Microsoft.Extensions.DependencyInjection.dll',
    '/_framework/_bin/Microsoft.Extensions.Logging.Abstractions.dll',
    '/_framework/_bin/Microsoft.Extensions.Logging.dll',
    '/_framework/_bin/Microsoft.Extensions.Options.dll',
    '/_framework/_bin/Microsoft.Extensions.Primitives.dll',
    '/_framework/_bin/Microsoft.JSInterop.dll',
    '/_framework/_bin/Microsoft.Net.Http.Headers.dll',
    '/_framework/_bin/Mono.Security.dll',
    '/_framework/_bin/Mono.WebAssembly.Interop.dll',
    '/_framework/_bin/mscorlib.dll',
    '/_framework/_bin/Newtonsoft.Json.dll',
    '/_framework/_bin/System.Buffers.dll',
    '/_framework/_bin/System.Core.dll',
    '/_framework/_bin/System.Data.dll',
    '/_framework/_bin/System.dll',
    '/_framework/_bin/System.IO.Pipelines.dll',
    '/_framework/_bin/System.Memory.dll',
    '/_framework/_bin/System.Net.Http.dll',
    '/_framework/_bin/System.Numerics.dll',
    '/_framework/_bin/System.Numerics.Vectors.dll',
    '/_framework/_bin/System.Runtime.CompilerServices.Unsafe.dll',
    '/_framework/_bin/System.Runtime.Serialization.dll',
    '/_framework/_bin/System.Text.Encodings.Web.dll',
    '/_framework/_bin/System.Threading.Channels.dll',
    '/_framework/_bin/System.Threading.Tasks.Extensions.dll',
    '/_framework/_bin/System.Xml.dll',
    '/_framework/_bin/System.Xml.Linq.dll',
    '/_framework/wasm/mono.js',
    '/_framework/wasm/mono.wasm',
    '/_framework/blazor.boot.json',
    '/_framework/blazor.server.js',
    '/_framework/blazor.webassembly.js',

    // Static content
    '/elements.css',
    '/favicon.ico',
    '/icon-192x192.png',
    '/icon-512x512.png',
    '/index.html',
    '/loader.css',
    '/site.css',
    '/site.js',

    // Service Worker
    '/register.js',

    // Application Manifest (PWA)
    '/manifest.json'
];

const staticCacheName = 'blazorspa-635de64ce70c4ad3b01abd9b844aa523';

self.addEventListener('install', event => {
    self.skipWaiting();
    event.waitUntil(
        caches.open(staticCacheName)
            .then(cache => {
                return cache.addAll(filesToCache);
            })
    );
});

self.addEventListener('fetch', event => {
    const requestUrl = new URL(event.request.url);

    // First, handle requests for the root path - server up index.html
    if (requestUrl.origin === location.origin) {
        if (requestUrl.pathname === '/') {
            event.respondWith(caches.match('/index.html'));
            return;
        }
    }
    // Anything else
    event.respondWith(
        // Check the cache
        caches.match(event.request)
            .then(response => {
                // anything found in the cache can be returned from there
                // without passing it on to the network
                if (response) {
                    console.log('Found ', event.request.url, ' in cache');
                    return response;
                }
                // otherwise make a network request
                return fetch(event.request)
                    .then(response => {
                        // if we got a valid response 
                        if (response.ok) {
                            // and the request was for something from our own app url
                            // we should add it to the cache
                            if (requestUrl.origin === location.origin) {

                                const pathname = requestUrl.pathname;
                                console.log("CACHE: Adding " + pathname);
                                return caches.open(staticCacheName).then(cache => {
                                    // you can only "read" a response once, 
                                    // but you can clone it and use that for the cache
                                    cache.put(event.request.url, response.clone());
                                });
                            }
                        }
                        return response;
                    });
            }).catch(error => {
                // handle this error - for now just log it
                console.log(error);
            })
    );
});