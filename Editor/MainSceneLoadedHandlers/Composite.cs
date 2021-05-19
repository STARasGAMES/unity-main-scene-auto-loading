using System;
using System.Collections.Generic;

namespace SaG.MainSceneAutoLoading.MainSceneLoadedHandlers
{
    public class Composite : IMainSceneLoadedHandler
    {
        private readonly List<IMainSceneLoadedHandler> _handlers;

        public Composite(List<IMainSceneLoadedHandler> handlers)
        {
            _handlers = handlers;
        }

        public void OnMainSceneLoaded(LoadMainSceneArgs args)
        {
            foreach (var handler in _handlers)
            {
                handler.OnMainSceneLoaded(args);
            }
        }
    }
}