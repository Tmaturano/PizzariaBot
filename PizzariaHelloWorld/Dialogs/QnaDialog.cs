using Microsoft.Bot.Builder.CognitiveServices.QnAMaker;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzariaHelloWorld.Dialogs
{
    [Serializable]
    [QnAMaker(subscriptionKey: "687fba498e4b4f6494062f70c93dc55d", knowledgebaseId: "ff27ec6b-6cf1-41f0-8f34-32a74b429d59", defaultMessage: "Frase padrão para quando não satisfazer ao índice mínimo de confiabilidade.", scoreThreshold: 0.5, top: 1)]
    public class QnaDialog : QnAMakerDialog
    {
        protected override async Task RespondFromQnAMakerResultAsync(IDialogContext context, IMessageActivity message, QnAMakerResults result)
        {
            var primeiraResposta = result.Answers.First().Answer;

            var resposta = ((Activity)context.Activity).CreateReply();

            var dadosResposta = primeiraResposta.Split(';');

            if (dadosResposta.Length == 1)
            {
                await context.PostAsync(primeiraResposta);
                return;
            }

            var titulo = dadosResposta[0];
            var descricao = dadosResposta[1];
            var urlSite = dadosResposta[2];
            var urlImagem = dadosResposta[3];

            var heroCard = new HeroCard
            {
                Title = titulo,
                Subtitle = descricao
            };

            heroCard.Buttons = new List<CardAction>
            {
                new CardAction(ActionTypes.OpenUrl, "Compre Agora", urlSite)
            };

            heroCard.Images = new List<CardImage>
            {
                new CardImage(url: urlImagem)
            };

            resposta.Attachments.Add(heroCard.ToAttachment());

            await context.PostAsync(resposta);

        }

    }
}