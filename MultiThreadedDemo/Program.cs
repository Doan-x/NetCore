/*
 * ----Foreground thread
+ Là luồng mặc định khi bạn tạo bằng new Thread(...).
+ Giữ cho ứng dụng còn sống:
+ Khi Main() (thread chính) kết thúc, chương trình chưa thoát ngay, 
    mà nó sẽ chờ tất cả các foreground thread chạy xong rồi mới thoát.
 */

/*
-------Background Thread
+ Là luồng được đánh dấu bằng t.IsBackground = true;
+ Không giữ ứng dụng sống:
+ Nếu chỉ còn background thread, 
    CLR (Common Language Runtime) sẽ kết thúc tiến trình ngay lập tức, 
    không quan tâm thread đó đã chạy xong hay chưa.
*/
class Program
{
    public static bool interrupt = false;
    // main => thread chính
    public static void Main(string[] args)
    {
        CancellationTokenSource cts = new CancellationTokenSource();
        // Tạo thread => tạo một object thread => default foreground thread
        //var t1 = new Thread(
        //() =>
        //{

        //    while (!interrupt)
        //    {
        //        Console.WriteLine("Hello! 1 - sleep 1s");
        //        // Tạm dừng thread
        //        Thread.Sleep(1000);
        //    }
        //});
        //// Thread không tham số
        //var t2 = new Thread(new ThreadStart(PrintNoParameter));
        //var t2a = new Thread(PrintNoParameter2);

        // Thread Có tham số, nhưng chỉ 1 tham số kiểu object
        var t3 = new Thread(new ParameterizedThreadStart(PrintWithParameter));
        var t4 = new Thread(PrintWithParameter);
        var t5 = new Thread(PrintWithParameter);


        //t1.IsBackground = true;
        //t2.IsBackground = true;

        // Run thread => system bắt đầu tạo 1 thread mới
        //t1.Start(); // Hello! 1  sleep 1s
        //t2.Start(); // Hello world 1 sleep 1s
        //t2a.Start(); // Hello world 2 sleep 2s

        // đối tượng obj sẽ được truyền vào thread.
        //t3.Start("thread 3"); // Hello no name -  sleep 3s
        //t4.Start();          // Hello no name  - sleep 3s

        t4.Start(new DemoParam(){ Name = "Trong", DelayValue = 1000,CancellationToken= cts.Token });
        t5.Start(new DemoParam() { Name = "Doan", DelayValue = 2000, CancellationToken = cts.Token });

        Console.ReadLine();

        // Ngắt thread khi main thread dừng
        interrupt = true;
        // Sử dụng CancellationTokenSource để ngắt
        cts.Cancel();
        // Ngắt sau 5s
        cts.CancelAfter(5000);

    }
    public static void PrintNoParameter()
    {
        while (!interrupt) {
            Console.WriteLine("Hello world 1 - sleep 1s");
            Thread.Sleep(1000);
        }
        
    }
    public static void PrintNoParameter2()
    {
        while (!interrupt)
        {
            Console.WriteLine("Hello world 2 - sleep 2s");
            Thread.Sleep(2000);

        }

    }
    public static void PrintWithParameter(object? p)
    {
        var param = p as DemoParam;
        if (param != null) {
            while (!param.CancellationToken.IsCancellationRequested)
            {
                Console.WriteLine($"Hello {param.Name}! - sleep {(param.DelayValue) / 1000}s");
                Thread.Sleep(param.DelayValue);
            }
        }
        else
        {
            Console.WriteLine("Object is invalid");
        }
       
    }
}
public class DemoParam
{
    public string? Name { get; set; }
    public int DelayValue {  get; set; }
    public required CancellationToken CancellationToken { get; set; }
}
