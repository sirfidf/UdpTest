using UdpTest;

switch (args?.GetValue(0)?.ToString())
{
    case "server":
        Server.Start();
        break;
    case "client":
        Client.Start();
        break;
    default:
		break;
}
