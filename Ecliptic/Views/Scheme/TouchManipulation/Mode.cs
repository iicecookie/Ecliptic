namespace Ecliptic.Views
{
    enum TouchManipulationMode
    {
        None,
        PanOnly,
        IsotropicScale,     // includes panning
        AnisotropicScale,   // includes panning
        ScaleRotate,        // implies isotropic scaling  // MAIN
        ScaleDualRotate     // adds one-finger rotation
    }
}