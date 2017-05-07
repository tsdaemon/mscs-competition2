namespace DSC.Core.Utils
{
    public static class ObjectMapper
    {
        public static void MapWithPrefix(this object destination, object source, string prefix)
        {
            var destType = destination.GetType();
            var srcType = source.GetType();

            foreach(var pr in srcType.GetProperties())
            {
                var val = pr.GetValue(source);
                var pr2 = destType.GetField($"{prefix}{pr.Name}");
                if(pr2 != null)
                    pr2.SetValue(destination, val);
            }
        }
    }
}
