namespace Practice
{
    public abstract class GeographicEntity
    {
        protected GeographicEntity(string name) => Name = name;
        public string Name { get; protected set; }
        public double Area { get; protected set; }
        public int Population { get; protected set; }
    }
}
