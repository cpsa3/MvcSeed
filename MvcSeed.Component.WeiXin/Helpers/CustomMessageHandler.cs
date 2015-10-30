using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Context;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.MessageHandlers;
using System.IO;
using System.Xml.Linq;

namespace MvcSeed.Component.WeiXin.Helpers
{
    public class CustomMessageHandler : MessageHandler<MessageContext>
    {
        private readonly EventService _eventService = new EventService();

        public CustomMessageHandler(Stream inputStream, int maxRecordCount = 0)
            : base(inputStream, maxRecordCount)
        {
        }

        public CustomMessageHandler(XDocument requestDocument, int maxRecordCount = 0)
            : base(requestDocument, maxRecordCount)
        {
        }

        public override IResponseMessageBase OnEventRequest(IRequestMessageEventBase requestMessage)
        {
            var eventResponseMessage = base.OnEventRequest(requestMessage);
            switch (requestMessage.MsgType)
            {
                case RequestMsgType.Event:
                    {
                        var response = _eventService.GetResponseMessage(requestMessage);
                        return response;
                    }
            }

            //TODO: 对Event信息进行统一操作
            return eventResponseMessage;
        }

        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "欢迎关注此服务号";
            return responseMessage;
        }
    }
}
