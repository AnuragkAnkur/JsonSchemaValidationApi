namespace JsonValidationCoreWebApi.UnitTests
{
    public class Constants
    {
        public const string MessageTemplate =
            "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level}] {SourceContext:l} :{MemberName} -> {Message}{NewLine}{Exception}";

        public const string ValidJson = @"{
                                              'description': 'A person',
                                              'type': 'object',
                                              'properties': {
                                                'name': {'type':'string'},
                                                'hobbies': {
                                                  'type': 'array',
                                                  'items': {'type':'string'}
                                                }
                                              }
                                            }";

        public const string SpecialCharacterJson = @"{
                                              'description': 'œœ œ',
                                              'type': 'object',
                                              'properties': {
                                                'name': {'type':'string'},
                                                'hobbies': {
                                                  'type': 'array',
                                                  'items': {'type':'string'}
                                                }
                                              }
                                            }";

        public const string InvalidJson = @"{
                                              'description': 'A person',
                                              'type': 'object',
                                              'properties': 
                                                'name': {'type':'string'},
                                                'hobbies': {
                                                  'type': 'array'
                                                  'items': {'type':'string'}
                                                }
                                              }
                                            }";
    }
}
