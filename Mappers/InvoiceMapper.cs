using System.ComponentModel;
using System.Data;
using System.Reflection;

namespace BgTiendaFacturacionAPI.Mappers
{
    public class InvoiceMapper
    {
        public static DataTable ToDataTable<T>(IList<T> data)
        //public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

        public static DataTable ConvertListToDataTable<T>(IList<T> list)
        {
            DataTable dataTable = new DataTable();

            // Usar reflexión para obtener las propiedades de T
            PropertyInfo[] properties = typeof(T).GetProperties();

            // Crear las columnas en el DataTable, basadas en las propiedades de la clase T
            foreach (PropertyInfo prop in properties)
            {
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            // Agregar las filas
            foreach (T item in list)
            {
                DataRow row = dataTable.NewRow();
                foreach (PropertyInfo prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value; // Asignar DBNull si es nulo
                }
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

    }
}
