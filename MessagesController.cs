using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;
using System.Collections.Generic;

namespace BotApp
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                // calculate something for us to return
                int length = (activity.Text ?? string.Empty).Length;

                // return our reply to the user
                //Activity reply = activity.CreateReply($"You sent {activity.Text} which was {length} characters");
                //await connector.Conversations.ReplyToActivityAsync(reply);
                Activity reply = activity.CreateReply($"Okay, bye bye!");
                switch (activity.Text)
                {
                    case "bye":
                    case "quit":
                        
                        await connector.Conversations.ReplyToActivityAsync(reply);
                        break;

                    case "i wanna bake a cake for my husband's birthday":
                        reply = activity.CreateReply($"What kind of cake?");
                        await connector.Conversations.ReplyToActivityAsync(reply);
                        break;

                    case "tiramisu":
                        string message = "Those are the ingredients you need for the Tiramisu cake -- See recipe here.";
                        message += "\n1. Flour (1Kg) \n2. Milk (1L) \n3. Egg (1 dozen) \n4. Brandy (1L) \n5. Whipped Cream (100ml) \n6. Dark Chocolate (1 block) \n7. White Sugar (1Kg)";
                        reply = activity.CreateReply(message);
                        await connector.Conversations.ReplyToActivityAsync(reply);
                        break;

                    case "1 2 6":
                        reply = activity.CreateReply($"Flour (1Kg), Milk (1L), Dark Chocolate (1 block) have been added to your cart.");
                        await connector.Conversations.ReplyToActivityAsync(reply);
                        reply = activity.CreateReply($"Do you want to confirm your order?");
                        await connector.Conversations.ReplyToActivityAsync(reply);
                        break;

                    case "yes":
                        reply = activity.CreateReply($"Do you want us to deliver or self-collect?");
                        await connector.Conversations.ReplyToActivityAsync(reply);
                        break;

                    case "deliver":
                        reply = activity.CreateReply($"What is the address you want us to deliver to?");
                        await connector.Conversations.ReplyToActivityAsync(reply);
                        break;

                    case "nus hangar":
                        reply = activity.CreateReply($"Thank you! Your order will be delivered soon.");
                        await connector.Conversations.ReplyToActivityAsync(reply);
                        break;

                    default:
                        await Conversation.SendAsync(activity, () => new SimpleAlarmDialog());
                        break;
                }
                
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }

    [LuisModel("6e5a5b60-7a99-41e1-8c61-fb35484c1d2c", "66d627d532c8492a906d17c36aa7fef4")]
    [Serializable]
    public class SimpleAlarmDialog : LuisDialog<object>
    {
        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = "Hi! My name is Botler :) What can I help you with?\n";

            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("recommendation")]
        public async Task ShowRecommendation(IDialogContext context, LuisResult result)
        {
            
            string message = "Sure! Here are some cakes that I recommend.";
            await context.PostAsync(message);
            message = "Which do you like?";
            await context.PostAsync(message);
            context.Wait(MessageReceived);

            /*
                        var entitiesArray = result.Entities;
                                    var reply = context.MakeMessage();
                                    foreach (var entityItem in result.Entities)
                                    {
                                        reply.Text = entityItem.Type;
                                        if (entityItem.Type == "baby product")
                                        {

                                            switch (entityItem.Entity)
                                            {
                                                case "clarke":
                                                    reply.Text = "Clarke is the main character";
                                                    break;
                                                default:
                                                    reply.Text = "I don't know this character";
                                                    break;
                                            }
                                            await context.PostAsync(reply);
                                            context.Wait(MessageReceived);
                                        }
                                    } 
            */
        }


        [LuisIntent("recipe")]
        public async Task ShowRecipe(IDialogContext context, LuisResult result)
        {

            string message = "Those are the ingredients you need for the Tiramisu cake -- See recipe here.";
            await context.PostAsync(message);
            message = "Choose multiple ingredients to be added to the cart.";
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("confirm order")]
        public async Task ShowOrder(IDialogContext context, LuisResult result)
        {

            string message = "Do you want us to deliver or self-collect?";
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }


        [LuisIntent("home delivery")]
        public async Task AskDelivery(IDialogContext context, LuisResult result)
        {

            string message = "Do you want us to deliver or self-collect?";
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }



           // /*
            public SimpleAlarmDialog(ILuisService service = null)
              : base(service)
            {
            }
           // */
        }
    }