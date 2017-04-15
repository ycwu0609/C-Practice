using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class UdpServer
{	
	
	int num_of_regist = 0;
	string[] IP_Set = new string[100];
	string[] Account_Set = new string[100];
	string[] Password_Set = new string[100];
	
    public static void Main()
    {
        // 開啟伺服器的 5555 連接埠，用來接收任何傳送到本機的訊息。
        IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 5555);
        // 建立接收的 Socket，並使用 Udp 的 Datagram 方式接收。
        Socket newsock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        // 綁定 Socket (newsock) 與 IPEndPoint (ipep)，讓 Socket 接收 5555 埠的訊息。
        newsock.Bind(ipep);
        Console.WriteLine("Waiting for a client..."); // 顯示 Waiting for client ...。
        // 建立 Remote 物件以便取得封包的接收來源的 EndPoint 物件。
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0); 
        EndPoint Remote = (EndPoint)(sender);
        
		
		
		while(true) // 無窮迴圈，不斷接收訊息並顯示到螢幕上。
        {
            byte[] data = new byte[1024]; // 設定接收緩衝區的陣列變數。
            int recv = newsock.ReceiveFrom(data, ref Remote); // 接收對方傳來的封包。
            // 將該封包以 UTF8 的格式解碼為字串，並顯示到螢幕上。
			if(Encoding.UTF8.GetString(data, 0, recv) == "Account"){
				Console.WriteLine("Old Account"); 
				int i;
				recv = newsock.ReceiveFrom(data, ref Remote);
				for(i=0; i < num_of_regist; i++){
					Console.WriteLine(Account_Set[i]);
					if(Encoding.UTF8.GetString(data, 0, recv) == Account_Set[i]) {
						Console.WriteLine("Account Match");
						break;
					}
				}
				
				recv = newsock.ReceiveFrom(data, ref Remote);
				Console.WriteLine("Data: "+ Encoding.UTF8.GetString(data, 0, recv));
				if(Encoding.UTF8.GetString(data, 0, recv) == Password_Set[i]){
					Console.Write("receive data from " + Remote.ToString());
					IP_Set[i] = Remote.ToString();
					newsock.SendTo(Encoding.UTF8.GetBytes("Success"), Remote);
					continue;
				}
				newsock.SendTo(Encoding.UTF8.GetBytes("Fail."), Remote);
				recv = newsock.ReceiveFrom(data, ref Remote);
			}
			if(Encoding.UTF8.GetString(data, 0, recv) == "Create Account"){
				Console.WriteLine("New Account");
				recv = newsock.ReceiveFrom(data, ref Remote);
				Account_Set[num_of_regist] = Encoding.UTF8.GetString(data, 0, recv);
				Console.WriteLine(Account_Set[num_of_regist]);
				recv = newsock.ReceiveFrom(data, ref Remote);
				Password_Set[num_of_regist] = Encoding.UTF8.GetString(data, 0, recv);
				Console.WriteLine(Password_Set[num_of_regist]);
				IP_Set[num_of_regist] = Remote.ToString();
				
				num_of_regist++;
			}
			
				
            Console.WriteLine(Encoding.UTF8.GetString(data, 0, recv)); 
        }
    }
}