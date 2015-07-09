using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Client
{
    public class JellyRecord<TEntity>
    {
        private string _jsonData;
        private TEntity _entity;

        public JellyRecord(string jsonData)
        {
            _jsonData = jsonData;
            _entity = JsonConvert.DeserializeObject<TEntity>(_jsonData);
        }

        public JellyRecord(TEntity entity)
        {
            _entity = entity;
        }

        public TEntity Entity { get { return _entity; } }

        public string GetSerializedData()
        {
            if(_entity == null) throw new InvalidOperationException("Can not get json text of an entity that is null");
            var jsonText = JsonConvert.SerializeObject(_entity);
            return jsonText;
        }
    }
}
