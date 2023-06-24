using System;
using System.ComponentModel;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace WPFZeroVersion.Common
{
    [Serializable]
    public class PropertyChangeBase : INotifyPropertyChanged
    {
        [field: NonSerialized()]
        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        public T Clone<T>(T RealObject)
        {
            using (Stream objectStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter?.Serialize(objectStream, RealObject);
                objectStream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(objectStream);
            }
        }
    }

    static class PropertyChangedBaseEx
    {
        public static void OnPropertyChanged<T, TProperty>(this T PropertyChangeBase, Expression<Func<T, TProperty>> propertyname) where T : PropertyChangeBase
        {
            if (propertyname.Body is MemberExpression PropertyName)
            {
                PropertyChangeBase.OnPropertyChanged(PropertyName.Member.Name);
            }
        }
    }
}
