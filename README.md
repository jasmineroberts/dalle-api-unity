
![alt text]()
 

 ## Overview
This is a simple DALL·E API wrapper that implements the API calls found in the OpenAI API Reference as Coroutines and Async functions. Additionally, there are 3 finished sample scenes for reference.

The syntax follows the docs as closely as possible.

Note: This is a community library and not officially affiliated with either [Unity](https://unity.com/) or [OpenAI](https://openai.com/).




## Set-Up

Download the Unity Editor. This application was developed in [Unity 2021.3.6f1 (LTS)](https://unity3d.com/unity/whats-new/2021.3.6)

**Note**: If this is your first time using the Unity Editor, install the [Unity Hub](https://public-cdn.cloud.unity3d.com/hub/prod/UnityHubSetup.dmg).


To run this project, you will need to input API and organization keys. 
These variables are located in `OpenAIDataModel.cs` found in `Projects >> Assets >> Scripts`. 

`API_KEY`

`ORGANIZATION_KEY`

To obtain the API and organization keys, [login](https://auth0.openai.com/u/login/) to your OpenAI account. If you do not have an account, create a free account. 
To find the API key, navigate to  `Personal >> API Keys` (in the upper right hand corner) or [here](https://beta.openai.com/account/api-keys).

Generate a new key. You will get a prompt saying "Please save this secret key somewhere safe and accessible. For security reasons, you won't be able to view it again through your OpenAI account. If you lose this secret key, you'll need to generate a new one."
Store this API key somewhere safe. 

To find the organization key, navigate to Organization Settings. The organization key is the `Organization ID` and will read `org-xxxxxxxxxxxxxxxxxxxxxxxx`.


## Scenes 

This project contains the following 3 scenes:


`Text to Image` replicates the standard DALL·E interface 

`Text to 3D Material` generates images as materials within a 3D environment 

`Outpainting` uses a Unity-specific implementation of ["outpainting"](https://openai.com/blog/dall-e-introducing-outpainting/)

*After setting the prompt press the generate button. It will fetch data from OpenAI server. You can not set the prompt again until you reset. But you can generate/(enlarge existing image) as much as you want from the same prompt and existing image.*

`Text to 3D Skybox` creates panorama via Outpainting (be very intentional with prompting) (W.I.P.)

`Skybox Result` has the last generated panorama (W.I.P.)


The above scenes can be located in `Projects >> Assets >> Scenes`
In all of the scenes the user write an image description and then presses the 'Generate' button. This calls the OpenAI API and preview the provided image. 

The images are downloaded automatically when they load from the server. They are in the application's persistent data path. You can easily add any path in the function parameters. Currently, the outputs are stored in `Project >> Assets >> OpenAI Generated Assets` folder.



## General Notes

Generated images are sqaure and have sizes of 256x256, 512x512, or 1024x1024 pixels. 

Smaller sizes are faster to generate. (Consider experimenting with the 360° skybox scene)

You can request at maximum 10 images at a time.

For more information about DALL·E API, see below. 

## API Reference

The above example contains template scenes using the DALL·E API.
For more advanced use cases, please refer to the [OpenAI API Documentation](https://beta.openai.com/docs/guides/images).


## Authors

- [Jasmine Roberts](https://www.twiiter.com/jasminezroberts)

## License 
[MIT License](https://opensource.org/licenses/MIT)

