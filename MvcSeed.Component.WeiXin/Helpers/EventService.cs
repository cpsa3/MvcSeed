using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Helpers;
using System;

namespace MvcSeed.Component.WeiXin.Helpers
{
    public class EventService
    {
        public ResponseMessageBase GetResponseMessage(IRequestMessageEventBase requestMessage)
        {
            ResponseMessageBase responseMessage = null;
            switch (requestMessage.Event)
            {
                case Event.ENTER:
                    {
                        var strongResponseMessage = requestMessage.CreateResponseMessage<ResponseMessageText>();
                        strongResponseMessage.Content = "您刚才发送了ENTER事件请求。";
                        responseMessage = strongResponseMessage;
                        break;
                    }
                case Event.LOCATION:
                    throw new Exception("暂不可用");
                case Event.subscribe:
                    {
                        break;
                    }
                case Event.unsubscribe:
                    {
                        var strongResponseMessage = requestMessage.CreateResponseMessage<ResponseMessageText>();
                        var openId = requestMessage.FromUserName;

                        strongResponseMessage.Content = "有空再来";
                        responseMessage = strongResponseMessage;
                        break;
                    }
                case Event.CLICK:
                    {
                        var requestMessageEvent_Click = requestMessage as RequestMessageEvent_Click;

                        if (requestMessageEvent_Click != null)
                        {
                            var eventKey = requestMessageEvent_Click.EventKey;
                            var response = requestMessage.CreateResponseMessage<ResponseMessageText>();
                            response.Content = "openId:" + requestMessage.FromUserName;
                            responseMessage = response;
                        }

                        break;
                    }
                case Event.VIEW:
                    {
                        responseMessage = requestMessage.CreateResponseMessage<ResponseMessageNews>();
                        var openId = requestMessage.FromUserName;
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return responseMessage;
        }
    }
}
