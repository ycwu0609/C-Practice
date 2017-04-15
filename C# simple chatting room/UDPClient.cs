using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class UdpClient
{
    public static void Main(string[] args)    // 主程式開始
    {
        // 連接到 args[0] 參數所指定的 Server，使用連接埠 5555
        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(args[0]), 5555);
        // 建立 Socket，連接到 Internet (InterNetwork)，使用 Udp 協定的 Datagram 方式 (Dgram)。
        Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
		int Enter = 0;
		EndPoint Remote = (EndPoint)(ipep);
		
		/*
		String strHostName = Dns.GetHostName();                                     //先讀取本機名稱
		IPHostEntry iphostentry = Dns.GetHostEntry(strHostName);   //取得本機的 IpHostEntry 類別實體
		string My_IP="";
		foreach (IPAddress ipaddress in iphostentry.AddressList) {
			Console.WriteLine(ipaddress.ToString());  //使用了兩種方式都可以讀取出IP位置  
		    My_IP = My_IP+ipaddress.ToString();
		}
		*/
		
		string My_Account = "";
		while(Enter == 0){	
			Console.WriteLine("Have an account? Enter \"1\" if you have one, else enter \"2\".");
			string Regist = Console.ReadLine();
			if(Regist == "1"){
				Console.WriteLine("Account: ");
				server.SendTo(Encoding.UTF8.GetBytes("Account"), ipep);
				string Account = Console.ReadLine();
				server.SendTo(Encoding.UTF8.GetBytes(Account), ipep); 
				Console.WriteLine("Password: ");
				server.SendTo(Encoding.UTF8.GetBytes("Password"), ipep);
				string Password = Console.ReadLine(); 
				server.SendTo(Encoding.UTF8.GetBytes(Password), ipep);
				
				byte[] data = new byte[1024];
				int recv = server.ReceiveFrom(data, ref Remote);
				string recv_data = Encoding.UTF8.GetString(data, 0, recv);
				
				if(recv_data == "Success") Enter = 1;
				My_Account = Account;
			}
			else if(Regist == "2"){
				server.SendTo(Encoding.UTF8.GetBytes("Create Account"), ipep);
				Console.WriteLine("Create Account: ");
				string New_Account = Console.ReadLine(); 
				server.SendTo(Encoding.UTF8.GetBytes(New_Account), ipep);
				Console.WriteLine("Your Password: ");
				string New_Password = Console.ReadLine(); 
				server.SendTo(Encoding.UTF8.GetBytes(New_Password), ipep);
				Console.WriteLine("Create Success!");
				
				My_Account = New_Account;
				Enter = 1;
			}
			else Console.WriteLine("Enter \"1\" if you have one, else enter \"2\".");
		}
		
		
        while(true)     // 不斷讀取鍵盤輸入，並傳送輸入訊息給伺服器。
        {
            string input = Console.ReadLine();    // 讀取鍵盤輸入
            if (input == "exit")    // 如果輸入為 exit，則強制離開程式。
                break;
			input = ": " + input;
			input = My_Account + input;
			
            server.SendTo(Encoding.UTF8.GetBytes(input), ipep);    // 將訊息以 UTF8 的方式編碼後傳出。
        }
        Console.WriteLine("Stopping client");    // 印出 Stopping client 訊息。
        server.Close();    // 關閉連線。
    }
}