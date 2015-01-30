namespace DocProcessingWorkflow
{
    using System;
    using System.Configuration;
    using System.Reflection;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Autofac;
    using Autofac.Integration.Mvc;
    using DocProcessingWorkflow.App_Start;
    using IdentityWrapper.Identity;
    using IdentityWrapper.Interfaces;
    using Logging;
    using Logging.NLog;
    using ServiceInterfaces;
    using Services;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var builder = new ContainerBuilder();
            RegisterTypes(builder);
            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            RouteTable.Routes.MapMvcAttributeRoutes();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private static void RegisterTypes(ContainerBuilder builder)
        {
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.Register(c => new NLogLogger()).As<ILogger>().InstancePerHttpRequest();

            String ctor = ConfigurationManager.ConnectionStrings["BBOS"].ConnectionString;
            String adf = ConfigurationManager.ConnectionStrings["ADF"].ConnectionString;

            var configurationManagerWrapperAssembly = Assembly.Load("ConfigurationManagerWrapper");
            builder.RegisterAssemblyTypes(configurationManagerWrapperAssembly).AsImplementedInterfaces();

            var businessEngineAssemblies = Assembly.Load("BusinessEngines");
            builder.RegisterAssemblyTypes(businessEngineAssemblies).AsImplementedInterfaces();

            var serviceAssemblies = Assembly.Load("Services");
            builder.RegisterAssemblyTypes(serviceAssemblies).AsImplementedInterfaces().InstancePerHttpRequest();

            builder.RegisterType<UserService>().As<IUserService>().InstancePerDependency();

            var repositoryAssemblies = Assembly.Load("DocProcessingRepository");
            builder.RegisterAssemblyTypes(repositoryAssemblies).AsImplementedInterfaces().WithParameter(new NamedParameter("connectionString", ctor)).InstancePerHttpRequest();

            builder.RegisterType<DocProcessingRepository.Repositories.PackStoreRepository>()
             .As<DocProcessingRepository.Interfaces.IPackStoreRepository>()
                //.WithParameter(new NamedParameter("path", oneStepDirectory));
            .WithParameter(new NamedParameter("connectionString", adf)).InstancePerHttpRequest();

            var identityAssembly = Assembly.Load("IdentityWrapper");
            builder.RegisterAssemblyTypes(identityAssembly).AsImplementedInterfaces()
                .WithParameter(new NamedParameter("connectionString", ctor))
                .WithParameter(new NamedParameter("context", new HttpContextWrapper(HttpContext.Current)))
                .InstancePerHttpRequest();

            var fileAssembly = Assembly.Load("SystemFileAdapter");
            builder.RegisterAssemblyTypes(fileAssembly).AsImplementedInterfaces();

            String oneStepDirectory = HttpContext.Current.Server.MapPath("~/Content/img/bg");

            builder.RegisterType<FileRepository.Repositories.OneStepFileRepository>()
                   .As<FileRepository.Interfaces.IOneStepFileRepository>()
                   .WithParameter(new NamedParameter("path", oneStepDirectory));

            String reportFileDirectory = HttpContext.Current.Server.MapPath("~/Content/img/bg");

            builder.RegisterType<FileRepository.Repositories.ReportFileRepository>()
                 .As<FileRepository.Interfaces.IReportFileRepository>()
                 .WithParameter(new NamedParameter("path", reportFileDirectory));

            String inputFileDirectory = HttpContext.Current.Server.MapPath("~/Content/img/bg");

            builder.RegisterType<FileRepository.Repositories.InputFileRepository>()
                   .As<FileRepository.Interfaces.IInputFileRepository>()
                   .WithParameter(new NamedParameter("path", inputFileDirectory));

            builder.RegisterType<FileRepository.Repositories.SubmitJobFileRepository>()
                   .As<FileRepository.Interfaces.ISubmitJobFileRepository>()
                   .WithParameter(new NamedParameter("path", inputFileDirectory));

            builder.RegisterType<FileRepository.Repositories.PdfFileRepository>()
                  .As<FileRepository.Interfaces.IPdfFileRepository>()
                  .WithParameter(new NamedParameter("path", inputFileDirectory));

            builder.RegisterType<UserManagerProvider>().As<IUserManagerProvider>().WithParameter(new NamedParameter("connectionString", ctor)).InstancePerDependency();

            builder.RegisterFilterProvider();
        }       
    }
}
