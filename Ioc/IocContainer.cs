using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Ioc
{/// <summary>
/// our ioc for our application
/// </summary>
    public static class IocContainer
    {
        /// <summary>
        /// kenel for our conatiner
        /// </summary>
        public static IKernel Kenel { get; private set; } = new StandardKernel();
        /// <summary>
        /// set up  for ioc container, binds all information required and ie readty for use
        /// Note: must be caaled as soon as the application starts up
        /// </summary>
        public static void  AppDomainSetup()
        {
            //bind all required view models
            BindViewModels();
        }

        /// <summary>
        /// bind all singleton models
        /// </summary>
        private static void BindViewModels()
        {
            //bind to a single instance
            Kenel.Bind<AppViewModel>().ToConstant(new AppViewModel());
        }
    }
}
