using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SpriteHelper : MonoBehaviour
{
    [MenuItem("Pokemon Tools/Slice Sprites: 64x64")]
    static void SliceSprites64()
    {
        // Change the below for the with and height dimensions of each sprite within the spritesheets
        int slice_width = 64;
        int slice_height = 64;

        // Change the below for the path to the folder containing the sprite sheets (warning: not tested on folders containing anything other than just spritesheets!)
        // Ensure the folder is within 'Assets/Resources/' (the below example folder's full path within the project is 'Assets/Resources/ToSlice')
        string folder_path = "To_Slice";

        Object[] sprite_sheets = Resources.LoadAll(folder_path, typeof(Texture2D));
        Debug.Log("sprite_sheets.Length: " + sprite_sheets.Length);

        for (int z = 0; z < sprite_sheets.Length; z++)
        {

            string path = AssetDatabase.GetAssetPath(sprite_sheets[z]);
            TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;
            ti.isReadable = true;
            ti.spriteImportMode = SpriteImportMode.Multiple;

            List<SpriteMetaData> new_data = new List<SpriteMetaData>();

            Texture2D sprite_sheet = sprite_sheets[z] as Texture2D;

            int sprites_sliced = 0;
            for (int i = 0; i < sprite_sheet.width; i += slice_width)
            {
                for (int j = sprite_sheet.height; j > 0; j -= slice_height)
                {
                    SpriteMetaData smd = new SpriteMetaData();
                    smd.pivot = new Vector2(0.5f, 0.5f);
                    smd.alignment = 9;
                    smd.name = sprite_sheets[z].name + '_' + (sprites_sliced++).ToString();
                    smd.rect = new Rect(i, j - slice_height, slice_width, slice_height);

                    new_data.Add(smd);
                }
            }

            ti.spritesheet = new_data.ToArray();
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }
        Debug.Log("Done Slicing!");
    }

    [MenuItem("Pokemon Tools/Slice Sprites: By Height")]
    static void SliceSpritesByHeight()
    {
        int raw_width;
        int raw_height;

        int slice_width;
        int slice_height;

        // Change the below for the path to the folder containing the sprite sheets (warning: not tested on folders containing anything other than just spritesheets!)
        // Ensure the folder is within 'Assets/Resources/' (the below example folder's full path within the project is 'Assets/Resources/ToSlice')
        string folder_path = "To_Slice";

        Object[] sprite_sheets = Resources.LoadAll(folder_path, typeof(Texture2D));
        Debug.Log("sprite_sheets.Length: " + sprite_sheets.Length);

        for (int z = 0; z < sprite_sheets.Length; z++)
        {
            string path = AssetDatabase.GetAssetPath(sprite_sheets[z]);
            TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;
            ti.isReadable = true;
            ti.spriteImportMode = SpriteImportMode.Multiple;

            List<SpriteMetaData> new_data = new List<SpriteMetaData>();

            // Get raw texture size
            Texture2D temp_texture = new Texture2D(1, 1);
            byte[] temp_bytes = File.ReadAllBytes(path);
            temp_texture.LoadImage(temp_bytes);
            raw_width = temp_texture.width;
            raw_height = temp_texture.height;

            slice_width = raw_height;
            slice_height = raw_height;

            Texture2D sprite_sheet = sprite_sheets[z] as Texture2D;

            int sprites_sliced = 0;
            for (int i = 0; i < raw_width; i += slice_width)
            {
                for (int j = raw_height; j > 0; j -= slice_height)
                {
                    SpriteMetaData smd = new SpriteMetaData();
                    smd.pivot = new Vector2(0.5f, 0.5f);
                    smd.alignment = 9;
                    smd.name = sprite_sheets[z].name + '_' + (sprites_sliced++).ToString();
                    smd.rect = new Rect(i, j - slice_height, slice_width, slice_height);

                    new_data.Add(smd);
                }
            }

            ti.spritesheet = new_data.ToArray();
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }
        Debug.Log("Done Slicing!");
    }
}