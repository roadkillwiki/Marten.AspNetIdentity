using System;
using System.Collections.Concurrent;

namespace Marten.AspNetIdentity.Tests.Integration
{
	public class DocumentStoreManager
	{
		private static readonly ConcurrentDictionary<string, IDocumentStore> _documentStores = new ConcurrentDictionary<string, IDocumentStore>();
		public static string ConnectionString => "host=localhost;port=5432;database=aspnetidentity;username=aspnetidentity;password=aspnetidentity;";

		public static IDocumentStore GetMartenDocumentStore(Type testClassType, string connectionString = null)
		{
			string documentStoreSchemaName = "";

			if (testClassType != null)
				documentStoreSchemaName = testClassType.Name;

			if (!_documentStores.ContainsKey(documentStoreSchemaName))
			{
				IDocumentStore docStore = Extensions.CreateDocumentStore(connectionString ?? ConnectionString, documentStoreSchemaName);
				_documentStores.TryAdd(documentStoreSchemaName, docStore);
			}

			return _documentStores[documentStoreSchemaName];
		}
	}
}