// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
//
// Author: Kenneth James Pouncey

#if MONOMAC
using MonoMac.OpenGL;
#elif WINDOWS || LINUX
using OpenTK.Graphics.OpenGL;
#elif GLES
using OpenTK.Graphics.ES20;
using TextureUnit = OpenTK.Graphics.ES20.All;
using TextureTarget = OpenTK.Graphics.ES20.All;
#endif

namespace Microsoft.Xna.Framework.Graphics
{
    public sealed partial class SamplerStateCollection
    {
        private void PlatformSetSamplerState(int index)
        {
        }

        private void PlatformClear()
        {
        }

        private void PlatformDirty()
        {
        }

        internal void PlatformSetSamplers(GraphicsDevice device)
        {
            for (var i = 0; i < _samplers.Length; i++)
            {
                var sampler = _samplers[i];
                var texture = device.Textures[i];

                if (sampler != null && texture != null)
                {
                    SamplerState lastSampler = null;
                    texture.glLastSamplerStates.TryGetValue(i, out lastSampler);
                    if (sampler != lastSampler)
                    {
                        // TODO: Avoid doing this redundantly (see TextureCollection.SetTextures())
                        // However, I suspect that rendering from the same texture with different sampling modes
                        // is a relatively rare occurrence...
                        GL.ActiveTexture(TextureUnit.Texture0 + i);
                        GraphicsExtensions.CheckGLError();

                        // NOTE: We don't have to bind the texture here because it is already bound in
                        // TextureCollection.SetTextures(). This, of course, assumes that SetTextures() is called
                        // before this method is called. If that ever changes this code will misbehave.
                        // GL.BindTexture(texture.glTarget, texture.glTexture);
                        // GraphicsExtensions.CheckGLError();

                    	sampler.Activate(device, texture.glTarget, texture.LevelCount > 1);
                        texture.glLastSamplerStates[i] = sampler;
                    }
                }
            }
        }
	}
}
