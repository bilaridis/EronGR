using DatabaseWrapper.SqlServer;
using Indexer;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Watson.ORM.Core;
using Watson.ORM.SqlServer;

namespace EronNew.Models
{

    /// <summary>
    /// IndexEngine is a lightweight RawData and text indexing platform written in C# targeted to both .NET Core and .NET Framework.  
    /// IndexEngine uses Sqlite as a storage repository for index data.  
    /// IndexEngine does NOT provide storage of the original RawDatas.
    /// </summary>
    public class MyIndexEngine : IDisposable, IMyIndexEngine
    {

        #region Public-Members

        /// <summary>
        /// Method to invoke when logging.
        /// </summary>
        public Action<string> Logger = null;

        /// <summary>
        /// Set the maximum number of threads that can be instantiated to process RawDatas.
        /// </summary>
        public int MaxIndexingThreads
        {
            get
            {
                return _MaxThreads;
            }
            set
            {
                if (value < 1) throw new ArgumentException("MaxThreads must be one or greater.");
                _MaxThreads = value;
            }
        }

        /// <summary>
        /// Get the number of threads currently processing RawDatas.
        /// </summary>
        public int CurrentIndexingThreads
        {
            get
            {
                return _CurrentThreads;
            }
        }

        /// <summary>
        /// Get a list of strings containing the user-supplied GUIDs of each of the RawDatas being processed.
        /// </summary>
        public IEnumerable<string> RawDatasIndexing
        {
            get
            {
                lock (_Lock)
                {
                    IEnumerable<string> ret = _RawDatasIndexing;
                    return ret;
                }
            }
        }

        /// <summary>
        /// Minimum character length for a term to be indexed.
        /// </summary>
        public int TermMinimumLength
        {
            get
            {
                return _TermMinimumLength;
            }
            set
            {
                if (value < 1) throw new ArgumentOutOfRangeException("TermMinimumLength must be greater than zero.");
                _TermMinimumLength = value;
            }
        }

        /// <summary>
        /// Delimiters to use when identifying terms in a RawData.  
        /// </summary>
        public char[] TermDelimiters
        {
            get
            {
                return _TermDelimiters;
            }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(TermDelimiters));
                _TermDelimiters = value;
            }
        }

        /// <summary>
        /// List of words to ignore when indexing.
        /// </summary>
        public List<string> IgnoreWords
        {
            get
            {
                return _IgnoreWords;
            }
            set
            {
                if (value == null)
                {
                    _IgnoreWords = new List<string>();
                }
                else
                {
                    _IgnoreWords = value;
                }
            }
        }



        #endregion

        #region Private-Members

        private string _Header = "[IndexEngine] ";
        private CancellationTokenSource _TokenSource = new CancellationTokenSource();
        private CancellationToken _Token;

        //private string _DbFilename = null;
        private DatabaseSettings _DbSettings = null;
        private WatsonORM _ORM = null;

        private int _MaxThreads = 32;
        private int _CurrentThreads = 0;
        private readonly object _Lock = new object();
        private List<string> _RawDatasIndexing = new List<string>();
        private int _TermMinimumLength = 3;

        #endregion

        #region Constructors-and-Factories

        public MyIndexEngine()
        {

        }

        public void Load(DatabaseSettings dbSettings)
        {
            //if (String.IsNullOrEmpty(databaseFile)) throw new ArgumentNullException(databaseFile);

            _Token = _TokenSource.Token;
            //_DbFilename = databaseFile;
            _DbSettings = dbSettings;

            _ORM = new WatsonORM(_DbSettings);
            _CurrentThreads = 0;

            _ORM.InitializeDatabase();
            //_ORM.TruncateTable(typeof(RawData));
            _ORM.InitializeTable(typeof(Document));
            //_ORM.TruncateTable(typeof(IndexEntry));
            _ORM.InitializeTable(typeof(IndexEntry));
            //_ORM.TruncateTable(typeof(Person));
            //_ORM.InitializeTable(typeof(Person));
            //_ORM.TruncateTable(typeof(Person));

            //#region Create-and-Store-Records

            //Person p1 = new Person("Abraham", "Lincoln", Convert.ToDateTime("1/1/1980"), null, 42, null, "initial notes p1", PersonType.Human, null, false);
            //Person p2 = new Person("Ronald", "Reagan", Convert.ToDateTime("2/2/1981"), Convert.ToDateTime("3/3/1982"), 43, 43, "initial notes p2", PersonType.Cat, PersonType.Cat, true);
            //Person p3 = new Person("George", "Bush", Convert.ToDateTime("3/3/1982"), null, 44, null, "initial notes p3", PersonType.Dog, PersonType.Dog, false);
            //Person p4 = new Person("Barack", "Obama", Convert.ToDateTime("4/4/1983"), Convert.ToDateTime("5/5/1983"), 45, null, "initial notes p4", PersonType.Human, null, true);

            //for (int i = 0; i < 8; i++) Console.WriteLine("");
            //Console.WriteLine("| Creating p1");
            //p1 = _ORM.Insert<Person>(p1);

            //for (int i = 0; i < 8; i++) Console.WriteLine("");
            //Console.WriteLine("| Creating p2");
            //p2 = _ORM.Insert<Person>(p2);

            //for (int i = 0; i < 8; i++) Console.WriteLine("");
            //Console.WriteLine("| Creating p3");
            //p3 = _ORM.Insert<Person>(p3);

            //for (int i = 0; i < 8; i++) Console.WriteLine("");
            //Console.WriteLine("| Creating p4");
            //p4 = _ORM.Insert<Person>(p4);

            //#endregion

            //#region Exists-Count-Sum

            //for (int i = 0; i < 8; i++) Console.WriteLine("");
            //DbExpression existsExpression = new DbExpression(_ORM.GetColumnName<Person>(nameof(Person.Id)), DbOperators.GreaterThan, 0);
            //Console.WriteLine("| Checking existence of records: " + _ORM.Exists<Person>(existsExpression));

            //for (int i = 0; i < 8; i++) Console.WriteLine("");
            //DbExpression countExpression = new DbExpression(_ORM.GetColumnName<Person>(nameof(Person.Id)), DbOperators.GreaterThan, 2);
            //Console.WriteLine("| Checking count of records: " + _ORM.Count<Person>(countExpression));

            //for (int i = 0; i < 8; i++) Console.WriteLine("");
            //Console.WriteLine("| Checking sum of ages: " + _ORM.Sum<Person>(_ORM.GetColumnName<Person>(nameof(Person.Age)), existsExpression));

            //#endregion


            //_ORM.Query("PRAGMA journal_mode = TRUNCATE");
        }
        #endregion

        #region Public-Methods

        /// <summary>
        /// Tear down IndexEngine and dispose of resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Add a RawData to the index.
        /// </summary>
        /// <param name="RawData">RawData.</param>
        public void Add(Document RawData)
        {
            Add(RawData, null);
        }

        /// <summary>
        /// Add a RawData to the index with tags.
        /// </summary>
        /// <param name="RawData">RawData.</param>
        /// <param name="tags">Tags.</param>
        public void Add(Document RawData, List<string> tags)
        {
            if (RawData == null) throw new ArgumentNullException(nameof(RawData));

            lock (_Lock)
            {
                _RawDatasIndexing.Add(RawData.GUID);
            }

            Task.Run(() => AddRawDataToIndex(RawData, tags), _Token);
        }

        /// <summary>
        /// Add a RawData to the index asynchronously.
        /// </summary>
        /// <param name="RawData">RawData.</param>
        /// <returns>Task.</returns>
        public async Task AddAsync(Document RawData)
        {
            await AddAsync(RawData, null);
        }

        /// <summary>
        /// Add a RawData to the index with tags, asynchronously.
        /// </summary>
        /// <param name="RawData">RawData.</param>
        /// <param name="tags">Tags.</param>
        /// <returns>Task.</returns>
        public async Task AddAsync(Document RawData, List<string> tags)
        {
            if (RawData == null) throw new ArgumentNullException(nameof(RawData));

            lock (_Lock)
            {
                _RawDatasIndexing.Add(RawData.GUID);
            }

            await Task.Run(() => AddRawDataToIndex(RawData, tags), _Token);
        }

        /// <summary>
        /// Search the index.
        /// </summary>
        /// <param name="terms">Search terms.</param>
        /// <returns>List of RawDatas.</returns>
        public List<Document> Search(List<string> terms)
        {
            return Search(terms, null, null, null);
        }

        /// <summary>
        /// Search the index.
        /// </summary>
        /// <param name="terms">Search terms.</param>
        /// <param name="indexStart">Index of results from which to begin returning records.</param>
        /// <param name="maxResults">Maximum number of records to return.</param>
        /// <returns>List of RawDatas.</returns>
        public List<Document> Search(List<string> terms, int? indexStart, int? maxResults)
        {
            return Search(terms, indexStart, maxResults, null);
        }

        /// <summary>
        /// Search the index.
        /// </summary>
        /// <param name="terms">Search terms.</param>       
        /// <param name="indexStart">Index of results from which to begin returning records.</param>
        /// <param name="maxResults">Maximum number of records to return.</param>
        /// <param name="filter">Database filters.</param> 
        /// <returns>List of RawDatas.</returns>
        public List<Document> Search(List<string> terms, int? indexStart, int? maxResults, DbExpression filter)
        {
            if (terms == null || terms.Count < 1) throw new ArgumentNullException(nameof(terms));

            #region Retrieve-RawData-GUIDs

            List<string> guids = GetRawDataGuidsByTerms(terms, indexStart, maxResults, filter);
            if (guids == null || guids.Count < 1)
            {
                Log("no RawData GUIDs found for the supplied terms");
                return new List<Document>();
            }

            #endregion

            #region Retrieve-and-Return

            DbExpression e = new DbExpression(_ORM.GetColumnName<Document>(nameof(Document.GUID)), DbOperators.In, guids);
            List<Document> ret = _ORM.SelectMany<Document>(indexStart, maxResults, e);
            Log("returning " + ret.Count + " RawDatas for search query");
            return ret;

            #endregion
        }

        /// <summary>
        /// Get RawData GUIDs that contain supplied terms.
        /// </summary>
        /// <param name="terms">List of terms.</param> 
        /// <returns>List of RawData GUIDs.</returns>
        public List<string> GetRawDataGuidsByTerms(List<string> terms)
        {
            return GetRawDataGuidsByTerms(terms, null, null, null);
        }

        /// <summary>
        /// Get RawData GUIDs that contain supplied terms.
        /// </summary>
        /// <param name="terms">List of terms.</param>
        /// <param name="indexStart">Index of results from which to begin returning records.</param>
        /// <param name="maxResults">Maximum number of records to return.</param>
        /// <param name="filter">Database filters.</param>
        /// <returns>List of RawData GUIDs.</returns>
        public List<string> GetRawDataGuidsByTerms(List<string> terms, int? indexStart, int? maxResults, DbExpression filter)
        {
            if (terms == null || terms.Count < 1) throw new ArgumentNullException(nameof(terms));

            List<string> ret = new List<string>();
            DbExpression e = new DbExpression(_ORM.GetColumnName<IndexEntry>(nameof(IndexEntry.Term)), DbOperators.In, terms);
            if (filter != null) e.PrependAnd(filter);

            List<IndexEntry> entries = _ORM.SelectMany<IndexEntry>(indexStart, maxResults, e);
            if (entries != null && entries.Count > 0)
            {
                ret = entries.Select(entry => entry.DocumentGuid).ToList();
                ret = ret.Distinct().ToList();
            }

            Log("returning " + ret.Count + " RawData GUIDs for terms query");
            return ret;
        }

        /// <summary>
        /// Delete RawData by its GUID.
        /// </summary>
        /// <param name="guid">GUID.</param>
        public void DeleteRawDataByGuid(string guid)
        {
            if (String.IsNullOrEmpty(guid)) throw new ArgumentNullException(nameof(guid));
            Log("deleting RawData GUID " + guid);
            DbExpression eIndexEntries = new DbExpression(_ORM.GetColumnName<IndexEntry>(nameof(IndexEntry.DocumentGuid)), DbOperators.Equals, guid);
            DbExpression eDocs = new DbExpression(_ORM.GetColumnName<Document>(nameof(Document.GUID)), DbOperators.Equals, guid);
            _ORM.DeleteMany<IndexEntry>(eIndexEntries);
            _ORM.DeleteMany<Document>(eDocs);
            return;
        }

        /// <summary>
        /// Delete RawData by its handle.
        /// </summary>
        /// <param name="handle">Handle.</param>
        public void DeleteRawDataByHandle(string handle)
        {
            if (String.IsNullOrEmpty(handle)) throw new ArgumentNullException(nameof(handle));
            Log("deleting RawDatas with handle " + handle);
            Document curr = GetRawDataByHandle(handle);
            if (curr == null) return;

            DbExpression eIndexEntries = new DbExpression(_ORM.GetColumnName<IndexEntry>(nameof(IndexEntry.DocumentGuid)), DbOperators.Equals, curr.GUID);
            DbExpression eDocs = new DbExpression(_ORM.GetColumnName<Document>(nameof(Document.GUID)), DbOperators.Equals, curr.GUID);
            _ORM.DeleteMany<IndexEntry>(eIndexEntries);
            _ORM.DeleteMany<Document>(eDocs);
            return;
        }

        /// <summary>
        /// Get a RawData by its GUID.
        /// </summary>
        /// <param name="guid">GUID.</param>
        /// <returns>RawData.</returns>
        public Document GetRawDataByGuid(string guid)
        {
            if (String.IsNullOrEmpty(guid)) throw new ArgumentNullException(nameof(guid));
            DbExpression e = new DbExpression(_ORM.GetColumnName<Document>(nameof(Document.GUID)), DbOperators.Equals, guid);
            Document doc = _ORM.SelectFirst<Document>(e);
            if (doc == null)
            {
                Log("RawData with GUID " + guid + " not found");
                return null;
            }
            return doc;
        }

        /// <summary>
        /// Get a RawData by its handle.
        /// </summary>
        /// <param name="handle">Handle.</param>
        /// <returns>RawData.</returns>
        public Document GetRawDataByHandle(string handle)
        {
            if (String.IsNullOrEmpty(handle)) throw new ArgumentNullException(nameof(handle));
            DbExpression e = new DbExpression(_ORM.GetColumnName<Document>(nameof(Document.Handle)), DbOperators.Equals, handle);
            Document doc = _ORM.SelectFirst<Document>(e);
            if (doc == null)
            {
                Log("RawData with handle " + handle + " not found");
                return null;
            }
            return doc;
        }

        /// <summary>
        /// Check if a RawData has been indexed by its handle.
        /// </summary>
        /// <param name="handle">Handle.</param>
        /// <returns>True if the RawData exists in the index.</returns>
        public bool IsHandleIndexed(string handle)
        {
            if (String.IsNullOrEmpty(handle)) throw new ArgumentNullException(nameof(handle));
            DbExpression e = new DbExpression(_ORM.GetColumnName<Document>(nameof(Document.Handle)), DbOperators.Equals, handle);
            return _ORM.Exists<Document>(e);
        }

        /// <summary>
        /// Check if a RawData has been indexed by its GUID.
        /// </summary>
        /// <param name="guid">GUID</param>
        /// <returns>True if the RawData exists in the index.</returns>
        public bool IsGuidIndexed(string guid)
        {
            if (String.IsNullOrEmpty(guid)) throw new ArgumentNullException(nameof(guid));
            DbExpression e = new DbExpression(_ORM.GetColumnName<Document>(nameof(Document.GUID)), DbOperators.Equals, guid);
            return _ORM.Exists<Document>(e);
        }

        /// <summary>
        /// Get the number of references for a given term.
        /// </summary>
        /// <param name="term">Term.</param>
        /// <returns>Reference count.</returns>
        public long GetTermReferenceCount(string term)
        {
            if (String.IsNullOrEmpty(term)) throw new ArgumentNullException(nameof(term));
            DbExpression e = new DbExpression(_ORM.GetColumnName<IndexEntry>(nameof(IndexEntry.Term)), DbOperators.Equals, term.ToLower());
            List<IndexEntry> entries = _ORM.SelectMany<IndexEntry>(e);
            if (entries != null && entries.Count > 0)
            {
                return entries.Sum(entry => entry.ReferenceCount);
            }
            else
            {
                return 0;
            }
        }

        #endregion

        #region Private-Methods

        /// <summary>
        /// Tear down IndexEngine and dispose of resources.
        /// </summary>
        /// <param name="disposing">True if disposing of resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Log("disposing");

                _TokenSource.Cancel();
                _ORM.Dispose();

                lock (_Lock)
                {
                    _RawDatasIndexing = null;
                }
            }
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private async Task AddRawDataToIndex(Document doc, List<string> tags)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (doc == null) throw new ArgumentNullException(nameof(doc));

            string header = "[" + doc.GUID + "] ";
            Log(header + "beginning processing");

            DateTime startTime = DateTime.Now;
            DateTime endTime = DateTime.Now;
            int termsRecorded = 0;

            try
            {
                #region Setup

                _CurrentThreads++;
                Dictionary<string, int> terms = new Dictionary<string, int>();

                #endregion

                #region Process-Tags

                if (tags != null && tags.Count > 0)
                {
                    Log(header + "processing tags");

                    foreach (string curr in tags)
                    {
                        if (String.IsNullOrEmpty(curr)) continue;
                        if (!terms.ContainsKey(curr.ToLower()))
                        {
                            terms.Add(curr.ToLower(), 1);
                        }
                        else
                        {
                            int refcount = terms[curr.ToLower()];
                            refcount = refcount + 1;
                            terms.Remove(curr.ToLower());
                            terms.Add(curr.ToLower(), refcount);
                        }
                    }
                }

                Log(header + "finished processing tags");

                #endregion

                #region Process-Content

                Log(header + "processing content");

                string content = Encoding.UTF8.GetString(doc.Data);

                string[] termsRaw = content.Split(_TermDelimiters, StringSplitOptions.RemoveEmptyEntries);
                List<string> termsAlphaOnly = new List<string>();

                if (termsRaw != null && termsRaw.Length > 0)
                {
                    foreach (string curr in termsRaw)
                    {
                        if (String.IsNullOrEmpty(curr)) continue;
                        if (curr.Length < _TermMinimumLength) continue;
                        if (_IgnoreWords.Contains(curr.ToLower())) continue;

                        string currAlphaOnly = AlphaOnlyString(curr);
                        if (String.IsNullOrEmpty(currAlphaOnly)) continue;
                        termsAlphaOnly.Add(currAlphaOnly);
                    }

                    if (termsAlphaOnly != null && termsAlphaOnly.Count > 0)
                    {
                        foreach (string curr in termsAlphaOnly)
                        {
                            if (!terms.ContainsKey(curr.ToLower()))
                            {
                                terms.Add(curr.ToLower(), 1);
                            }
                            else
                            {
                                int refcount = terms[curr.ToLower()];
                                refcount = refcount + 1;
                                terms.Remove(curr.ToLower());
                                terms.Add(curr.ToLower(), refcount);
                            }
                        }
                    }

                    Log(header + "extracted terms");
                }
                else
                {
                    Log(header + "no terms found");
                }

                #endregion

                #region Remove-Existing-Entries

                DeleteRawDataByGuid(doc.GUID);
                Log(header + "deleting existing RawDatas with GUID " + doc.GUID);

                DeleteRawDataByHandle(doc.Handle);
                Log(header + "deleting existing RawDatas with handle " + doc.Handle);

                #endregion

                #region Create-New-RawData-Entry

                doc = _ORM.Insert<Document>(doc);
                Log(header + "created RawData entry");

                #endregion

                #region Create-New-Terms-Entries

                Log(header + "creating " + terms.Count + " index entries, please be patient");
                foreach (KeyValuePair<string, int> term in terms)
                {
                    IndexEntry entry = new IndexEntry(doc.GUID, term.Key, term.Value);
                    entry = _ORM.Insert<IndexEntry>(entry);
                    termsRecorded++;
                }

                #endregion

                return;
            }
            catch (TaskCanceledException)
            {
                Log(header + "cancellation requested");
            }
            catch (OperationCanceledException)
            {
                Log(header + "cancellation requested");
            }
            catch (Exception e)
            {
                Log(header + "exception encountered: " + Environment.NewLine + e.ToString());
                throw;
            }
            finally
            {
                _CurrentThreads--;

                lock (_Lock)
                {
                    if (_RawDatasIndexing != null
                        && _RawDatasIndexing.Count > 0
                        && _RawDatasIndexing.Contains(doc.GUID))
                    {
                        _RawDatasIndexing.Remove(doc.GUID);
                    }
                }

                endTime = DateTime.Now;
                TimeSpan ts = (endTime - startTime);

                decimal msTotal = Convert.ToDecimal(ts.TotalMilliseconds.ToString("F"));
                decimal msPerTerm = 0;
                if (termsRecorded > 0) msPerTerm = Convert.ToDecimal((msTotal / termsRecorded).ToString("F"));

                Log(header + "finished; " + termsRecorded + " terms [" + msTotal + "ms total, " + msPerTerm + "ms/term]");
            }
        }

        private string AlphaOnlyString(string dirty)
        {
            if (String.IsNullOrEmpty(dirty)) return null;
            string clean = null;
            for (int i = 0; i < dirty.Length; i++)
            {
                int val = (int)(dirty[i]);

                if (
                    ((val > 64) && (val < 91))          // A...Z
                    || ((val > 96) && (val < 123))      // a...z
                    || ((val > 912) && (val < 976))
                    )
                {
                    clean += dirty[i];
                }
            }

            return clean;
        }

        private void Log(string msg)
        {
            Logger?.Invoke(_Header + msg);
        }

        #endregion

        #region Private-Static

        private static char[] _TermDelimiters = new char[]
            {
                '!',
                '\"',
                '#',
                '$',
                '%',
                '&',
                '\'',
                '(',
                ')',
                '*',
                '+',
                ',',
                '-',
                '.',
                '/',
                ':',
                ';',
                '<',
                '=',
                '>',
                '?',
                '@',
                '[',
                '\\',
                ']',
                '^',
                '_',
                '`',
                '{',
                '|',
                '}',
                '~',
                ' ',
                '\'',
                '\"',
                '\u001a',
                '\r',
                '\n',
                '\t'
            };

        private static List<string> _IgnoreWords = new List<string>
        {
"post"
,"ownerid"
,"typeid"
,"title"
,"basic"
,"pricetotal"
,"null"
,"square"
,"zone"
,"area"
,"parkingarea"
,"bedroom"
,"bathroom"
,"createddate"
,"updateddate"
,"description"
,"active"
,"false"
,"deleted"
,"sold"
,"reserved"
,"condition"
,"constructionyear"
,"energyefficiency"
,"floor"
,"furniture"
,"parkingareatype"
,"petallowed"
,"renovationyear"
,"view"
,"searchrawkey"
,"bde"
,"e"
,"c"
,"ab"
,"stateofpost"
,"owner"
,"type"
,"extrainformation"
,"relatedposts"
,"edittab"
,"newtab",
            "a",
            "about",
            "above",
            "after",
            "again",
            "against",
            "aint",
            "ain't",
            "all",
            "also",
            "am",
            "an",
            "and",
            "any",
            "are",
            "arent",
            "aren't",
            "as",
            "at",
            "be",
            "because",
            "been",
            "before",
            "being",
            "below",
            "between",
            "both",
            "but",
            "by",
            "cant",
            "can't",
            "cannot",
            "could",
            "couldnt",
            "couldn't",
            "did",
            "didnt",
            "didn't",
            "do",
            "does",
            "doesnt",
            "doesn't",
            "doing",
            "dont",
            "don't",
            "down",
            "during",
            "each",
            "few",
            "for",
            "from",
            "further",
            "had",
            "hadnt",
            "hadn't",
            "has",
            "hasnt",
            "hasn't",
            "have",
            "havent",
            "haven't",
            "having",
            "he",
            "hed",
            "he'd",
            "he'll",
            "hes",
            "he's",
            "her",
            "here",
            "heres",
            "here's",
            "hers",
            "herself",
            "him",
            "himself",
            "his",
            "how",
            "hows",
            "how's",
            "i",
            "id",
            "i'd",
            "i'll",
            "im",
            "i'm",
            "ive",
            "i've",
            "if",
            "in",
            "into",
            "is",
            "isnt",
            "isn't",
            "it",
            "its",
            "it's",
            "its",
            "itself",
            "lets",
            "let's",
            "me",
            "more",
            "most",
            "mustnt",
            "mustn't",
            "my",
            "myself",
            "no",
            "nor",
            "not",
            "of",
            "off",
            "on",
            "once",
            "only",
            "or",
            "other",
            "ought",
            "our",
            "ours",
            "ourselves",
            "out",
            "over",
            "own",
            "same",
            "shall",
            "shant",
            "shan't",
            "she",
            "she'd",
            "she'll",
            "shes",
            "she's",
            "should",
            "shouldnt",
            "shouldn't",
            "so",
            "some",
            "such",
            "than",
            "that",
            "thats",
            "that's",
            "the",
            "their",
            "theirs",
            "them",
            "themselves",
            "then",
            "there",
            "theres",
            "there's",
            "these",
            "they",
            "theyd",
            "they'd",
            "theyll",
            "they'll",
            "theyre",
            "they're",
            "theyve",
            "they've",
            "this",
            "those",
            "thou",
            "though",
            "through",
            "to",
            "too",
            "under",
            "until",
            "unto",
            "up",
            "very",
            "was",
            "wasnt",
            "wasn't",
            "we",
            "we'd",
            "we'll",
            "were",
            "we're",
            "weve",
            "we've",
            "werent",
            "weren't",
            "what",
            "whats",
            "what's",
            "when",
            "whens",
            "when's",
            "where",
            "wheres",
            "where's",
            "which",
            "while",
            "who",
            "whos",
            "who's",
            "whose",
            "whom",
            "why",
            "whys",
            "why's",
            "with",
            "wont",
            "won't",
            "would",
            "wouldnt",
            "wouldn't",
            "you",
            "youd",
            "you'd",
            "youll",
            "you'll",
            "youre",
            "you're",
            "youve",
            "you've",
            "your",
            "yours",
            "yourself",
            "yourselves"
        };

        #endregion
    }

}
