using UnityEngine;
using UnityEditor;

public class TexturePostProcessor : AssetPostprocessor
{
    /*
    void OnPreprocessTexture()
    {
        TextureImporter importer = (TextureImporter)assetImporter;
        //importer.textureFormat = TextureImporterFormat.DXT5Crunched;
        importer.maxTextureSize = 1024;
        importer.npotScale = TextureImporterNPOTScale.None;

        //importer.npotScale = TextureImporterNPOTScale.ToLarger;
        //importer.npotScale = TextureImporterNPOTScale.None;
        //importer.textureFormat = TextureImporterFormat.AutomaticCrunched;

        //importer.compressionQuality = 50;

        //importer.generateCubemap = TextureImporterGenerateCubemap.None;
        importer.mipmapEnabled = false;



        
        //textureImporter.convertToNormalmap = true;
        //importer.npotScale = TextureImporterNPOTScale.None;
        //importer.filterMode = FilterMode.Point;
        //importer.textureFormat = TextureImporterFormat.ARGB32;

    }

    void OnPostprocessTexture(Texture2D texture)
    {
        TextureImporter importer = assetImporter as TextureImporter;
        //importer.textureType = TextureImporterType.Sprite;
        //importer.spritePixelsPerUnit = 2;
        //importer.anisoLevel = 0;

        //importer.filterMode = FilterMode.Trilinear;
        //importer.isReadable = true;


        Object asset = AssetDatabase.LoadAssetAtPath(importer.assetPath, typeof(Texture2D));

        
        

        /*
        if (asset)
        {
            EditorUtility.SetDirty(asset);
        }
        else
        {
            texture.anisoLevel = 0;
            texture.filterMode = FilterMode.Trilinear;
        }
    }
    */
}
