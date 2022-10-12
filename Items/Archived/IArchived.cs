using Terraria;

namespace TheConfectionRebirth.Items.Archived
{
	public interface IArchived
	{
		int ArchivatesTo();

		int ArchivatesTo(Item item) => ArchivatesTo();
	}
}
