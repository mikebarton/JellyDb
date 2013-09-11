using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;
using Castle.MicroKernel.Registration;

namespace JellyDb.Core.IoC
{
    public class WindsorResolver
    {
        private static IWindsorContainer container;

        static WindsorResolver()
        {
            container = new WindsorContainer();
        }

        public static T Resolve<T>()
        {
            return container.Resolve<T>();
        }

        public static T Resolve<T>(string key)
        {
            return container.Resolve<T>(key);
        }

        public static void Register(IRegistration[] registrations)
        {
            container.Register(registrations);
        }

        public static IWindsorContainer Container
        {
            get { return container; }
        }


    }
}
