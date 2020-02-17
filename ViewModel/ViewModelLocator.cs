using Ninject;
using POS.Ioc;


namespace POS
{
    /// <summary>
    /// view model locator 
    /// responsible for locating the global value to the views
    /// can bind to views
    /// </summary>
    public  class ViewModelLocator
    {
        public static ViewModelLocator Instance { get; private set; } = new ViewModelLocator();
        public static AppViewModel AppViewModel => IocContainer.Kenel.Get<AppViewModel>();
    }
}
