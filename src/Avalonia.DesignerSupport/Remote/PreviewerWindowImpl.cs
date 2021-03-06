﻿using System;
using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.Controls.Remote.Server;
using Avalonia.Platform;
using Avalonia.Remote.Protocol;
using Avalonia.Remote.Protocol.Viewport;
using Avalonia.Threading;

namespace Avalonia.DesignerSupport.Remote
{
    class PreviewerWindowImpl : RemoteServerTopLevelImpl, IWindowImpl, IEmbeddableWindowImpl
    {
        private readonly IAvaloniaRemoteTransportConnection _transport;

        public PreviewerWindowImpl(IAvaloniaRemoteTransportConnection transport) : base(transport)
        {
            _transport = transport;
            ClientSize = new Size(1, 1);
        }

        public void Show()
        {
        }

        public void Hide()
        {
        }

        public void BeginMoveDrag()
        {
        }

        public void BeginResizeDrag(WindowEdge edge)
        {
        }

        public Point Position { get; set; }
        public Action<Point> PositionChanged { get; set; }
        public Action Deactivated { get; set; }
        public Action Activated { get; set; }
        public IPlatformHandle Handle { get; }
        public WindowState WindowState { get; set; }
        public Size MaxClientSize { get; } = new Size(4096, 4096);
        public event Action LostFocus;

        protected override void OnMessage(IAvaloniaRemoteTransportConnection transport, object obj)
        {
            // In previewer mode we completely ignore client-side viewport size
            if (obj is ClientViewportAllocatedMessage alloc)
            {
                Dispatcher.UIThread.InvokeAsync(() => SetDpi(new Vector(alloc.DpiX, alloc.DpiY)));
                return;
            }
            base.OnMessage(transport, obj);
        }
        
        public void Resize(Size clientSize)
        {
            _transport.Send(new RequestViewportResizeMessage
            {
                Width = clientSize.Width,
                Height = clientSize.Height
            });
            ClientSize = clientSize;
            RenderIfNeeded();
        }

        public IScreenImpl Screen { get; } = new ScreenStub();

        public void Activate()
        {
        }
        
        public void SetTitle(string title)
        {
        }

        public IDisposable ShowDialog()
        {
            return Disposable.Empty;
        }

        public void SetSystemDecorations(bool enabled)
        {
        }

        public void SetIcon(IWindowIconImpl icon)
        {
        }

        public void ShowTaskbarIcon(bool value)
        {
        }
    }
}