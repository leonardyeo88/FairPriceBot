var express = require('express');
var bodyParser = require('body-parser');
var request = require('request');
var app = express();
var history = [];


app.use(bodyParser.urlencoded({extended: false}));
app.use(bodyParser.json());
app.listen((process.env.PORT || 3000));

// Server frontpage
app.get('/', function (req, res) {
    res.send('This is TestBot Server');
});

// Facebook Webhook
app.get('/webhook', function (req, res) {
    if (req.query['hub.verify_token'] === 'testbot_verify_token') {
        res.send(req.query['hub.challenge']);
    } else {
        res.send('Invalid verify token');
    }
});

// handler receiving messages
app.post('/webhook', function (req, res) {
    var events = req.body.entry[0].messaging;
    // var event = events[0];
    // sendMessage(event.sender.id, {text: "Hi, how can I help you?"});

    for (i = 0; i < events.length; i++) {
        var event = events[i];
        if (event.message && event.message.text) {
            var temp = event.message.text;
            temp = temp.toLowerCase();

            if(temp.includes('diaper')) {
                sendMessage(event.sender.id, {text:"Would you like to send an incident report to help Pampers improve on their diapers?"});
                sleep(1000);
                sendMessage(event.sender.id, {text:"Super-absorbent gel beads are commonly used in disposable diapers and in the packaging of many food products. These small gel beads absorb nearly 30 times their weight, to help lock moisture away from baby’s skin. Occasionally, you may see some of these gel beads on baby’s skin. They are safe and can be gently wiped away. To learn more about what’s in a diaper, read our article about diaper ingredients."});   
            }

            if (temp.includes('yes')){
                sendMessage(event.sender.id, {text: 'Sure thing!'});
            }

         
            if (temp.includes('cake')) {
               sendMessage(event.sender.id, {text: "Which one do you like?"});
               sleep(1000);
               sendMessage(event.sender.id, {text: "List:\n- Lemon Cake \n- Chocolate Cake \n- Tiramisu\n- Lava Cake \n- Matcha Cake \n- Fruit Cake \n- Strawberry Cake"});
               sleep(1000);
               sendMessage(event.sender.id, {text: "Sure! Here are some cakes that I recommend!"});
            }

            if (temp.includes('tiramisu')) {
                sendMessage(event.sender.id, {text: "Enter 'add to cart' and the item number of the ingredients you would like to order. If you would like more than one item, enter more and the item number. or else 'confirm order' to check out"});
                sleep(1000);
                sendMessage(event.sender.id, {text: "These are the ingredients you need for the Tiramisu cake \n- 3 cups of strong black coffee, preferably espresso, cooled \n- 3 tbsp caster sugar\n- 6 tbsp Amaretto liqueur.\n- 2 eggs, separated \n- 250g/8¾oz mascarpone cheese \n- 250ml/8¾ fl oz whipped cream \n- cocoa powder, to dust \n- 1 packet of Savoiardi (sponge lady finger biscuits)" });
                sleep(1000);         

                var imageUrl = "http://foodnetwork.sndimg.com/content/dam/images/food/fullset/2011/2/4/2/RX-FNM_030111-Sugar-Fix-005_s4x3.jpg.rend.sni12col.landscape.jpeg";

                message = {
                    "attachment": {
                        "type": "template",
                        "payload": {
                            "template_type": "generic",
                            "elements": [{
                                "title": "Tiramisu",
                                "subtitle": "Delicious Tiramisu",
                                "image_url": imageUrl ,
                                "buttons": [{
                                    "type": "web_url",
                                    "url": imageUrl,
                                    "title": "Tiramisu"
                                    }, {
                                    "type": "postback",
                                    "title": "I like this",
                                    "payload": "User " + " likes tiramisu " + imageUrl,
                                }]
                            }]
                        }
                    }
                };

                sendMessage(event.sender.id, message);

    
            }

            if (temp.includes('confirm order')){
                sendMessage(event.sender.id, {text: "Would you like for self-collection or home delivery?"});
            }

            if (temp.includes('collection')){
                sendMessage(event.sender.id, {text: "Great! Your order is being processed by Maria and will be ready for pick-up in 10 minutes"});
            }

            if (temp.includes('thanks')){
               sendMessage(event.sender.id, {text: "Sure Thing! Good luck with your baking and happy birthday to John!"});
            }
            
        }
    }
    res.sendStatus(200);
});

// generic function sending messages
function sendMessage(recipientId, message) {
    request({
        url: 'https://graph.facebook.com/v2.6/me/messages',
        qs: {access_token: process.env.PAGE_ACCESS_TOKEN},
        method: 'POST',
        json: {
            recipient: {id: recipientId},
            message: message,
        }
    }, function(error, response, body) {
        if (error) {
            console.log('Error sending message: ', error);
        } else if (response.body.error) {
            console.log('Error: ', response.body.error);
        }
    });

};

function sleep(milliseconds) {
  var start = new Date().getTime();
  for (var i = 0; i < 1e7; i++) {
    if ((new Date().getTime() - start) > milliseconds){
      break;
    }
  }
};

