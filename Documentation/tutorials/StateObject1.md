

# Creating a State Object

Demonstrates how to create a state object using any of the state object classes: [BlendState](T_Microsoft_Xna_Framework_Graphics_BlendState.md), [DepthStencilState](T_Microsoft_Xna_Framework_Graphics_DepthStencilState.md), [RasterizerState](T_Microsoft_Xna_Framework_Graphics_RasterizerState.md), or [SamplerState](T_Microsoft_Xna_Framework_Graphics_SamplerState.md).

# The Complete Sample

The code in this topic shows you the technique. You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download StateObject.zip](http://go.microsoft.com/fwlink/?LinkId=258731).

# Creating a State Object

### To create a state object

1.  Declare three state object variables as fields in your game.
    
    This example declares three rasterizer state objects and uses them to change the culling state.
    
    ```
    RasterizerState rsCullNone;
    ```
    
2.  Create a customizable state object.
    
    Create a state object from the [RasterizerState](T_Microsoft_Xna_Framework_Graphics_RasterizerState.md) class and initialize it by explicitly setting the cull mode.
    
    ```
    rsCullNone = new RasterizerState();
    rsCullNone.CullMode = CullMode.None;
    rsCullNone.FillMode = FillMode.WireFrame;
    rsCullNone.MultiSampleAntiAlias = false;
    ```
    
3.  Respond to the user pressing the A key on a gamepad to change the culling mode.
    
    The application starts with culling turned off; toggle between culling modes by pushing the A key on a gamepad. Unlike a customizable state object, use a built-in state object to create an object with a set of predefined state.
    
    ```
    if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
        changeState = true;
    
    if ((changeState) && (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Released))
    {
        if (GraphicsDevice.RasterizerState.CullMode == CullMode.None)
            GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
        else if (GraphicsDevice.RasterizerState.CullMode == CullMode.CullCounterClockwiseFace)
            GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;
        else if (GraphicsDevice.RasterizerState.CullMode == CullMode.CullClockwiseFace)
            GraphicsDevice.RasterizerState = rsCullNone;
    
        changeState = false;
    }
    ```
    
    The example contains two triangles. The first one is rendered if you select clockwise winding order; the second triangle is rendered if you select counterclockwise winding order; both triangles are rendered if you select no culling.
    

© 2012 Microsoft Corporation. All rights reserved.  
Version: 2.0.61024.0