﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Application.BusinessLogicLayer.MediatR;
using Application.BusinessLogicLayer.Modules.WordsCounter.RequestModels;
using Application.BusinessLogicLayer.Modules.WordsCounter.ResponseModels;
using Application.BusinessLogicLayer.Modules.WordsCounter.Services.Interfaces;
using Application.DataAccessLayer.Context;
using MediatR;

namespace Application.BusinessLogicLayer.Modules.WordsCounter.Queries
{
    public class GetBasicDataQuery : IRequest<GetBasicDataResponseModel>
    {
        public string InputText { get; }

        public GetBasicDataQuery(GetBasicDataRequestModel requestModel)
        {
            InputText = requestModel.InputText;
        }
    }

    public class GetBasicDataQueryHandler : QueryBase<GetBasicDataQuery, GetBasicDataResponseModel>
    {
        private readonly IWordService _wordService;

        public GetBasicDataQueryHandler(WordsCounterReadOnlyDbContext context, IWordService wordService) : base(context)
        {
            _wordService = wordService;
        }

        public override async Task<GetBasicDataResponseModel> Handle(GetBasicDataQuery request, CancellationToken cancellationToken)
        {
            GetBasicDataResponseModel responseModel = new GetBasicDataResponseModel
            {
                Characters = await Task.Run(() => GetCharactersCount(request.InputText), cancellationToken),
                CharactersWithoutSpaces = await Task.Run(() => GetCharactersWithoutSpacesCount(request.InputText), cancellationToken),
                Words = await Task.Run(() => GetWordsCount(request.InputText), cancellationToken),
                Sentences = await Task.Run(() => GetSentencesCount(request.InputText), cancellationToken)
            };

            return responseModel;
        }

        private static int GetCharactersCount(string inputText)
        {
            return inputText.Replace(Environment.NewLine, string.Empty).Length;
        }

        private static int GetCharactersWithoutSpacesCount(string inputText)
        {
            return inputText.Count(x => !char.IsWhiteSpace(x));
        }

        private int GetWordsCount(string inputText)
        {
            IEnumerable<string> words = _wordService.GetWords(inputText);

            return words.Count();
        }

        private static int GetSentencesCount(string inputText)
        {
            return Regex.Split(inputText, @"(?<=[\.!\?])\s+").Length;
        }
    }
}
