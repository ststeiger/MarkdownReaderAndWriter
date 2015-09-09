// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ColorCode.Common
{
    public class LanguageRepository : ILanguageRepository
    {
        private readonly Dictionary<string, ILanguage> loadedLanguages;
        // private readonly ReaderWriterLockSlim loadLock;
        private readonly object loadLock;

        public LanguageRepository(Dictionary<string, ILanguage> loadedLanguages)
        {
            this.loadedLanguages = loadedLanguages;
            // loadLock = new ReaderWriterLockSlim();
            loadLock = new object();
        }

        public IEnumerable<ILanguage> All
        {
            get { return loadedLanguages.Values; }
        }


        public ILanguage FindById(string languageId)
        {
            Guard.ArgNotNullAndNotEmpty(languageId, "languageId");
            
            ILanguage language = null;

            lock (loadLock)
            {

                bool bFound = false;

                foreach (var x in loadedLanguages)
                {
                    if (
                            (x.Key.ToLower() == languageId.ToLower())
                            ||
                            (x.Value.HasAlias(languageId))
                        )
                    {
                        bFound = true;
                        language = x.Value;
                        break;
                    }
                }

                if (!bFound)
                    language = default(ILanguage);

            }

            /*
            
            loadLock.EnterReadLock();
            
            
            try
            {
                // If we have a matching name for the language then use it
                // otherwise check if any languages have that string as an
                // alias. For example: "js" is an alias for Javascript.
                language = loadedLanguages.FirstOrDefault(x => (x.Key.ToLower() == languageId.ToLower()) ||
                                                               (x.Value.HasAlias(languageId))
                                                               ).Value;
            }
            finally
            {
                loadLock.ExitReadLock();
            }
            */
            return language;
        }

        public void Load(ILanguage language)
        {
            Guard.ArgNotNull(language, "language");

            if (string.IsNullOrEmpty(language.Id))
                throw new ArgumentException("The language identifier must not be null or empty.", "language");

            lock (loadLock)
            {
                loadedLanguages[language.Id] = language;
            }

            /*
            loadLock.EnterWriteLock();

            try
            {
                loadedLanguages[language.Id] = language;
            }
            finally
            {
                loadLock.ExitWriteLock();
            }
            */
        }
    }
}