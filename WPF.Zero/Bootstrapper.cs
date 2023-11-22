using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace WPF.Zero
{
    public class Bootstrapper : BootstrapperBase
    {
        /// <summary>IOC 容器</summary>
        private CompositionContainer container;


        public Bootstrapper()
        {
            Initialize();
        }

        protected override async void OnStartup(object sender, StartupEventArgs e)
        {
            await DisplayRootViewForAsync<IScreen>();
        }

        /// <summary>查找模块</summary>
        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            /// 若有其余模块可在此处添加
            /// 返回目前正在执行程序集，当View在目前正在执行的程序集中时，可以这样写。

            var lst = new List<Assembly>();
            lst.AddRange(base.SelectAssemblies());
            lst.AddRange(
                Directory.GetFiles(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\Plugins")
                .Where(file => file.EndsWith("dll", true, CultureInfo.CurrentCulture))
                .Select(Assembly.LoadFrom));

            return lst;
        }

        /// <summary>用MEF组合部件</summary>
        protected override void Configure()
        {
            container = new CompositionContainer(new AggregateCatalog(AssemblySource.Instance.Select(x => new AssemblyCatalog(x)).OfType<ComposablePartCatalog>()));


            /// 如果还有自己的部件都加载这个地方
            var batch = new CompositionBatch();
            batch.AddExportedValue<IWindowManager>(new WindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            batch.AddExportedValue(container);

            container.Compose(batch);
        }

        /// <summary>根据传过来的key或名称得到实例</summary>
        /// <param name="service"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected override object GetInstance(Type service, string key)
        {
            string contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(service) : key;
            var exports = container.GetExportedValues<object>(contract);

            var exportList = exports.ToList();/// 避免直接用exports时 调用2次IEnumerable操作
            if (exportList.Any())
            {
                return exportList.First();
            }

            throw new Exception(string.Format("找不到{0}实例.", contract));
        }

        /// <summary>获取某一特定类型的所有实例</summary>
        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetExportedValues<object>(AttributedModelServices.GetContractName(service));
        }

        /// <summary>将实例传递给 Ioc 容器，使依赖关系注入</summary>
        protected override void BuildUp(object instance)
        {
            container.SatisfyImportsOnce(instance);
        }
    }
}
