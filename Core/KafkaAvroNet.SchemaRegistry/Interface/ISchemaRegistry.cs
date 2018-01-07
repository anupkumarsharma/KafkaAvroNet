using KafkaAvroNet.SchemaRegistry.Models;

namespace KafkaAvroNet.SchemaRegistry.Interface
{
    public interface ISchemaRegistryProvider
    {


        /// <summary>
        /// Get the schema string identified by the input id.
        /// </summary>
        /// <param name="id">id (int) – the globally unique identifier of the schema</param>
        /// <returns>Response Object:
        /// - schema(string) – Schema string identified by the id
        /// </returns>
        Schema GetById(string id);


        /// <summary>
        /// Get a list of registered subjects.
        /// </summary>
        /// <returns>Array of subject names</returns>
        string[] GetAllSubjects();



        /// <summary>
        /// Get a list of versions registered under the specified subject.
        /// </summary>
        /// <param name="subject">the name of the subject</param>
        /// <returns>Array of versions of the schema registered under this subject</returns>
        int[] GetSchemaVersions(string subject);



        /// <summary>
        /// Get a specific version of the schema registered under this subject
        /// </summary>
        /// <param name="subject">Name of the subject</param>
        /// <param name="versionId">Version of the schema to be returned. Valid values for versionId are between [1,2^31-1]</param>
        SchemaMetadata GetBySubjectAndId(string subject, string versionId);






        /// <summary>
        /// Get a latest version of the schema registered under this subject (using /subjects/(string: subject)/versions/latest)
        /// Note that there may be a new latest schema that gets registered right after this request is served.
        /// </summary>
        /// <param name="subject">Name of the subject</param>
        SchemaMetadata GetLatestSchemaMetadata(string subject);

        
    }
}
