using System;
using System.Collections.Concurrent;
using Marten;

namespace MartenAspNetIdentity.Tests.Integration
{
	public class DocumentStoreManager
	{
		private static readonly ConcurrentDictionary<string, IDocumentStore> _documentStores = new ConcurrentDictionary<string, IDocumentStore>();
		public static string ConnectionString => "host=localhost;port=5432;database=aspnetidentity;username=aspnetidentity;password=aspnetidentity;";

		public static IDocumentStore GetMartenDocumentStore(Type testClassType)
		{
			string documentStoreSchemaName = "";

			if (testClassType != null)
				documentStoreSchemaName = testClassType.Name;

			if (!_documentStores.ContainsKey(documentStoreSchemaName))
			{
				IDocumentStore docStore = Extensions.CreateDocumentStore(ConnectionString, documentStoreSchemaName);
				_documentStores.TryAdd(documentStoreSchemaName, docStore);
			}

			return _documentStores[documentStoreSchemaName];
		}
	}
}