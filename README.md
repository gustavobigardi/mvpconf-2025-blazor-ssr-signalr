# MVPConf 2025 — Blazor SSR, Server‑Sent Events e SignalR

Repositório das demos e slides da palestra sobre aplicações reativas com Blazor usando Server‑Sent Events (SSE) e SignalR.

## Demos (pasta `demos/`)

- `server-sent-events/` — Demo de SSE em Blazor: API expõe um endpoint de eventos em `api/message` (content-type `text/event-stream`) e um cliente Blazor WebAssembly consome via `System.Net.ServerSentEvents`, formando um chat simples.
- `signalr/` — Demo de SignalR self‑hosted com Blazor: inclui o `ChatHub` (URL `/chathub`) e páginas cliente para enviar/receber mensagens em tempo real.
- `signalr-azure/` — Demo de SignalR usando Azure SignalR Service: a mesma aplicação do SignalR, adicionando `.AddAzureSignalR()` para usar o serviço gerenciado da Azure.

## Slides

Os slides da apresentação estão na pasta `slides/`.
