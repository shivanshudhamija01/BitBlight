namespace Features.Resources.Interfaces
{
    public interface IResourcePersistence
    {
        public InventorySaveData GetSaveData();
        public void LoadData(InventorySaveData data);
    }
}