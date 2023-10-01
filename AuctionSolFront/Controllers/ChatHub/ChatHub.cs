//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Http.Extensions;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.SignalR;



//namespace AuctionSolFront.Controllers.ChatHub
//{

//    public class ChatHub : Hub
//    {
//        private readonly IHttpContextAccessor _httpContextAccessor;

//        private readonly IUserConnectionManager _userConnectionManager;

//        public ChatHub(IHttpContextAccessor httpContextAccessor, IUserConnectionManager userConnectionManager)
//        {
//            _httpContextAccessor = httpContextAccessor;
//            _userConnectionManager = userConnectionManager;
//        }

//        public override async Task OnConnectedAsync()
//        {
//            try
//            {
//                var httpContext = Context.GetHttpContext();
//                if (httpContext != null)
//                {
//                    var userLoginId = httpContext.Request.Cookies["userId"];

//                    if (!string.IsNullOrEmpty(userLoginId))
//                    {
//                        _userConnectionManager.AddConnection(userLoginId, Context.ConnectionId);

//                        // Update user status to "online"
//                        _userConnectionManager.UpdateUserStatus(userLoginId, "online");

//                        var res = _userConnectionManager.GetUserStatuses();

//                        foreach (var item in res)
//                        {
//                            if (item.Value == "online")
//                            {
//                                var userId = item.Key;
//                                var status = "online";

//                                await Clients.All.SendAsync("UserStatusChanged", userId, status);
//                            }
//                        }
//                        await base.OnConnectedAsync();
//                    }
//                }


//            }
//            catch (Exception)
//            {
//                throw;
//            }

//        }

//        public override async Task OnDisconnectedAsync(Exception exception)
//        {
//            try
//            {
//                var httpContext = Context.GetHttpContext();
//                if (httpContext != null)
//                {
//                    var userIdCookie = httpContext.Request.Cookies["userId"];

//                    if (!string.IsNullOrEmpty(userIdCookie))
//                    {
//                        string userId = userIdCookie;
//                        _userConnectionManager.RemoveConnection(userId, Context.ConnectionId);
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                // Handle exceptions
//            }
//        }


//        public async Task SendMessage(string receiverId, string recipientName, string text, string senderId, string senderPicture, string staffId)
//        {
//            try
//            {
//                IEnumerable<string> sendToIds = _userConnectionManager.GetConnections(receiverId); //1413

//                //var message = new ChatMessageDto
//                //{
//                //    Text = text,
//                //    SentAt = DateTimeOffset.UtcNow,
//                //    ClientId = receiverId, //1422
//                //    SenderId = senderId,
//                //    SenderPicture = senderPicture
//                //};

//                //if (sendToIds.Any())
//                //{
//                //    foreach (var Id in sendToIds)
//                //    {
//                //        await Clients.Client(Id).SendAsync("ReceiveMessage", message.Text, message.SenderId, message.SenderPicture, message.ClientId);
//                //        await SaveMessage(message.Text, staffId, false);
//                //    }
//                //}
//                //else
//                //{
//                //    await SaveMessage(message.Text, staffId, false);
//                //}
//            }
//            catch (Exception)
//            {

//                throw;
//            }

//        }


//    }

//}


using Microsoft.AspNetCore.SignalR;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IUserConnectionManager _userConnectionManager;

        public ChatHub(IUserConnectionManager userConnectionManager)
        {
            _userConnectionManager = userConnectionManager;
        }
        public override async Task OnConnectedAsync()
        {
            try
            {
                var httpContext = Context.GetHttpContext();
                if (httpContext != null)
                {
                    var userLoginId = httpContext.Request.Cookies["userId"];

                    if (!string.IsNullOrEmpty(userLoginId))
                    {
                        _userConnectionManager.AddConnection(userLoginId, Context.ConnectionId);

                        // Update user status to "online"
                        _userConnectionManager.UpdateUserStatus(userLoginId, "Live");

                        var res = _userConnectionManager.GetUserStatuses();

                        foreach (var item in res)
                        {
                            if (item.Value == "Live")
                            {
                                var userId = item.Key;
                                var status = "Live";

                                await Clients.All.SendAsync("UserStatusChanged", userId, status);
                            }
                        }
                        await base.OnConnectedAsync();
                    }
                }

            }
            catch
            {

            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                var httpContext = Context.GetHttpContext();
                if (httpContext != null)
                {
                    var userIdCookie = httpContext.Request.Cookies["userId"];

                    if (!string.IsNullOrEmpty(userIdCookie))
                    {
                        string userId = userIdCookie;

                        _userConnectionManager.UpdateUserStatus(userId, "offline");
                        await Clients.All.SendAsync("UserStatusChanged", userId, "offline");
                        _userConnectionManager.RemoveConnection(userId, Context.ConnectionId);
                        
                    }
                }
               

            }
            catch (Exception ex)
            {
                // Handle exceptions
            }
        }

        public async Task SendMessage(string userId, string message)
        {
            try
            {
                // Get the connection IDs associated with the target user
                var connectionIds = _userConnectionManager.GetConnections(userId);

                // Send the message to each of the target user's connections
                foreach (var connectionId in connectionIds)
                {
                    await Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions, e.g., user not found, message not sent
            }
        }

    }
}
