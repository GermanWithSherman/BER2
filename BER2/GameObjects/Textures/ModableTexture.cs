using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

public abstract class ModableBinaryResource<T> : IModable
{
    protected T _binaryData;

    public string Path;

    public static implicit operator T(ModableBinaryResource<T> resource) => resource.Data;

    public T Data
    {
        get
        {
            if (_binaryData == null)
            {
                initialize();
            }
            return _binaryData;
        }
    }

    public abstract IModable copyDeep();

    public static V copyDeep<V>(V original) where V : ModableBinaryResource<T>, new()
    {
        var result = new V();
        result.Path = Modable.copyDeep(original.Path);
        return result;
    }

    protected abstract void initialize();

    public virtual void mod(IModable modable)
    {
        ModableBinaryResource<T> modData = modable as ModableBinaryResource<T>;
        if (modData == null)
            throw new Exception("Cast failed");
        Path = Modable.mod(Path, modData.Path);
    }

}

public class ModableTexture : ModableBinaryResource<ImageSource>, IModable, IMainContent
{

    public ModableTexture(string path, bool includeMods=true)
    {
        Path = System.IO.Path.Combine(GameManager.Instance.DataPath, path);

        if (!includeMods)
            return;

        foreach(string modPath in GameManager.Instance.pathsMods(path))
        {
            string absouluteModPath = System.IO.Path.Combine(GameManager.Instance.DataPath, modPath);
            if (File.Exists(absouluteModPath))
                Path = absouluteModPath;
        }

    }

    public ModableTexture()
    {
    }

    public override IModable copyDeep()
    {
        return ModableBinaryResource<ImageSource>.copyDeep(this);
    }

    protected override void initialize()
    {
        if (File.Exists(Path))
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(Path);
            bitmapImage.EndInit();
            _binaryData = bitmapImage;
        }
        else
        {
            //_texture = GameManager.Instance.TextureCache.MissingTexture;
        }
    }

    public override void mod(IModable modable)
    {
        base.mod(modable);
    }

    public UIElement getVisual()
    {
        Image result = new Image();

        result.Source = Data;

        return result;
    }
}
