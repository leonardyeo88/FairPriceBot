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
                        string message = "Those are the ingredients you need for the Tiramisu cake -- See recipe [here](http://www.epicurious.com/recipes/food/views/tiramisu-351138).";
                        message += "\n1. Flour (1Kg) \n2. Milk (1L) \n3. Egg (1 dozen) \n4. Brandy (1L) \n5. Whipped Cream (100ml) \n6. Dark Chocolate (1 block) \n7. White Sugar (1Kg) \n\n\n\n\n Enter the item numbers of the ingredients you would like to order.";
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
                        reply = activity.CreateReply($"Sure thing Jasmine! Your order will be delivered in 10 minutes. Good luck with the cake! :)");
                        await connector.Conversations.ReplyToActivityAsync(reply);
                        break;
                        
                    case "hi botler":
                        reply = activity.CreateReply($"Hi Jasmine! Why are you up so late? Do you want some food again?");
                        await connector.Conversations.ReplyToActivityAsync(reply);
                        break;

                    case "no my baby is crying again. i need milk":
                        reply = activity.CreateReply($"Enter confirm to purchase your previous milk order.");
                        await connector.Conversations.ReplyToActivityAsync(reply);
                        message = "![Friso](https://www.frisogold.com.my/sites/journey-of-friso-gold-milk/assets/batch_tracker/img/friso-tin.png)";
                        reply = activity.CreateReply(message);
                        await connector.Conversations.ReplyToActivityAsync(reply);
                        reply = activity.CreateReply($"Or enter change order to choose another brand.");
                        await connector.Conversations.ReplyToActivityAsync(reply);
                        break;
                        
                    case "confirm":
                        message = "Great. Your delivery will arrive in approximately 10 minutes via [FoodPanda](https://www.foodpanda.sg/).";
                        message += "Hang tight baby! Help is on the way. Botler to the rescue. :)";
                        reply = activity.CreateReply(message);
                        await connector.Conversations.ReplyToActivityAsync(reply);
                        
                        reply = activity.CreateReply($"");
                        await connector.Conversations.ReplyToActivityAsync(reply);
                        break;
                        
                        
                    case "there are beads from the diaper on my baby's skin?":
                        reply = activity.CreateReply($"Hey, Jasmine. Super-absorbent gel beads are commonly used in disposable diapers and in the packaging of many food products. These small gel beads absorb nearly 30 times their weight, to help lock moisture away from baby’s skin. Occasionally, you may see some of these gel beads on baby’s skin. They are safe and can be gently wiped away. To learn more about what’s in a diaper, read Pamper's [article](http://www.pampers.com/en-us/about-pampers/diapers-and-wipes/article/whats-in-a-pampers-diaper) about diaper ingredients.");
                        await connector.Conversations.ReplyToActivityAsync(reply);
                        reply = activity.CreateReply($"Would you like to send an incident report to help Pampers improve on their diapers?");
                        await connector.Conversations.ReplyToActivityAsync(reply);
                        break;
                    
                    case "thanks. yes please":
                        reply = activity.CreateReply($"Sure thing! :)");
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
            string message = "Hi! Jasmine! Botler here :) What can I help you with?\n";

            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }
        //test
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

        /*[LuisIntent("None")]
        public async Task AskMeaning(IDialogContext context, LuisResult result)
        {

            string message = "Please rephrase your statement?" + result.Query;
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }
        */
        // /*
        public SimpleAlarmDialog(ILuisService service = null)
              : base(service)
            {
            }
        // */
        }
    }
