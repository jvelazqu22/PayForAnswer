using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class GeneralHelper
    {
        public string GetTermWithNextLastLetter(string term)
        {
            if (string.IsNullOrWhiteSpace(term) || term.Length == 1) return term;

            string[] alphabet = General.Alphabet.Split(',');
            string lastCharacter = term.ElementAt(term.Length - 1).ToString();
            string nextLetterInAlphabet = string.Empty;
            string newTerm = string.Empty;

            if(alphabet.Contains(lastCharacter.ToLower()))
            {
                int indexCounter = 0;
                foreach(var letter in alphabet)
                {
                    if (letter.ToLower() == lastCharacter.ToLower())
                    {
                        nextLetterInAlphabet = alphabet[++indexCounter];
                        break;
                    }
                    indexCounter++;
                }
                newTerm = term.Substring(0, term.Length - 1) + nextLetterInAlphabet;
            }
            return newTerm.ToLower();
        }

        public string ReplaceDisAllowedCharacterWithPlaceHolders(string term)
        {
            foreach (var pair in SubjectEntityValues.DISALLOWED_KEY_FIELDS_DICTIONARY)
                if (term.Contains(pair.Key))
                    term = term.Replace(pair.Key, SubjectEntityValues.DISALLOWED_KEY_FIELDS_DICTIONARY[pair.Value]);
            return term;
        }

        public string ReplacePlaceHolderWithCharacter(string term)
        {
            foreach (var pair in SubjectEntityValues.DISALLOWED_KEY_FIELDS_DICTIONARY)
                if (term.Contains(pair.Value))
                    term = term.Replace(pair.Value, pair.Key);
            return term;
        }
    }
}
