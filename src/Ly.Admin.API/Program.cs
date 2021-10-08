using Autofac.Extensions.DependencyInjection;
using Serilog;

namespace Ly.Admin.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            #region  ���� Serilog
            #region �������ļ���ȡ
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("LYAdminConfig.serilog.json")
                .Build();
            #endregion

            Log.Logger = new LoggerConfiguration()
                //��С���������
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
            #region ���������ļ��Ļ��Ϳ�������Ĵ���
                // ������־���������̨
                //.WriteTo.Console()
                // ������־������ļ����ļ��������ǰ��Ŀ�� logs Ŀ¼��
                // �ռǵ���������Ϊÿ��
                //.WriteTo.File(Path.Combine("logs", @"log.txt"), rollingInterval: RollingInterval.Day)
            #endregion
            #region ��ȡ�����ļ���
                .ReadFrom.Configuration(configuration)
            #endregion
                .CreateLogger();
            #endregion

            var host = CreateHostBuilder(args).Build();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                //���� Autofac ��ʵ������ע��
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())

                // �� Serilog ����Ϊ��־��¼�ṩ����
                .UseSerilog(dispose: true)//dispose:true�˳�ʱ�ͷ���־����
                                          //����Զ��������ļ�
                .ConfigureAppConfiguration((context, configBuilder) =>
                {
                    //configBuilder.Sources.Clear();//���֮ǰ���еĻ�ָ���ļ�

                    //��ʱ����Ҫ��� ֻ�Ƕ��һ��
                    configBuilder.AddJsonFile("LYAdminConfig.options.json");
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }

    }
}
