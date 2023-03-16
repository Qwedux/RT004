# Documentation

## NOTICE: I don't use config file for scene description yet

## PerspectiveCamera

- Member variables:
  - `center` (*point*): Perspective center, i.e. where would all rays going through the camera near-plane meet.
  - `direction` (*vector*): Direction that the camera is facing towards, i.e. if we imagine the camera as a pyramid, than `direction` is the perpendicular from the `center` (top of the pyramid) to the base plane.
  - `up` (*vector*): Up-direction for the camera, i.e. what do we consider as going upwards.
  - `nearPlaneCenter` (*point*): Center of the near-plane.
  - `FOV` (*radians*): Horizontal field of view that the camera captures in radians, i.e. value $\pi/4$ would represent $90^\circ$. Valid values are bethween $0.1$ ($~5.72958^\circ$) and $\pi - 0.1$ ($~174.27^\circ$).
  - `imageWidth` (`int`): Number of pixels in the horizontal direction.
  - `imageHeight` (`int`): Number of pixels in the vertical direction.
  - `dx` (*vector*): Horizontal direction of the camera, i.e. the direction of the horizontal axis of the near-plane. It is normalized so that $-1\cdot dx$ is the left edge and $1\cdot dx$ is the right edge of the near-plane.
  - `dy` (*vector*): Vertical direction of the camera, i.e. the direction of the vertical axis of the near-plane. It is normalized so that $-1\cdot dy$ is the bottom edge and $1\cdot dy$ is the top edge of the near-plane.
- Methods:
  - Constructor: `PerspectiveCamera(Vector3 center, Vector3 direction, Vector3 up, float nearPlaneDistance, float FOV, int imageWidth, int imageHeight)`
  - GetRay: `public bool GetRay(float x, float y, out Vector3 p0, out Vector3 p1)` - Computes the ray origin (`p0`) and direction (`p1`) for the given pixel coordinates. Returns `true` if the ray is valid, `false` otherwise. The ray is valid if the pixel coordinates are within the image bounds, i.e. $-1 \leq x \leq 1$ and $-1 \leq y \leq 1$.

## Materials

- Member variables:
  - `color` (*color*): Color of the material.
  - `specularColor` (*color*): Specular color of the material.
  - `ambientCoefficient` (*float*): Ambient coefficient of the material.
  - `diffuseCoefficient` (*float*): Diffuse coefficient of the material.
  - `specularCoefficient` (*float*): Specular coefficient of the material.
  - `specularity` (*float*): Specularity of the material.

## Light Sources

### PointLight

- Member variables:
  - `position` (*point*): Position of the light source.
  - `color` (*color*): Color of the light source.
  - `intensity` (*float*): Intensity of the light source.
- Methods:
  - Constructor: `PointLight(Vector3 position, Vector3 color, float intensity)`
  - DirectionAtPoint: `public Vector3 DirectionAtPoint(Vector3 p)` - Computes the direction of light coming from light source to the given point `p`.
  - IntensityAtPoint: `public float IntensityAtPoint(Vector3 p)` - Computes the intensity of light coming from light source to the given point `p`.

### AmbientLight

- Member variables:
  - `color` (*color*): Color of the light source.
  - `intensity` (*float*): Intensity of the light source.
- Methods:
  - Constructor: `AmbientLight(Vector3 color, float intensity)`
  - DirectionAtPoint: `public Vector3 DirectionAtPoint(Vector3 p)` - Returns zero vector.
  - IntensityAtPoint: `public float IntensityAtPoint(Vector3 p)` - Returns the intensity of the light source.

## Solids

### Sphere

- Member variables:
  - `center` (*point*): Center of the sphere.
  - `radius` (*float*): Radius of the sphere.
  - `material` (*material*): Material of the sphere.
- Methods:
  - Constructor: `Sphere(Vector3 center, float radius, Material m)`
  - Intersect: `public bool Intersect(Vector3 p0, Vector3 p1, out float t)` - Computes the intersection of the ray with the sphere. Returns `true` if the ray intersects the sphere, `false` otherwise. If the ray intersects the sphere, the parameter `t` is set to the multiple of `p1` from the ray origin to the nearest intersection point.
  - GetNormal: `public Vector3 GetNormal(Vector3 p1, Vector3 p)` - Computes the normal at the given point `p` on the sphere assuming that `p1` is the direction of the ray that intersects the sphere at `p`.

### Plane

- Member variables:
  - `center` (*point*): Center of the plane.
  - `normal` (*vector*): Normal of the plane.
  - `material` (*material*): Material of the plane.
- Methods:
  - Constructor: `Plane(Vector3 center, Vector3 normal, Material m)`
  - Intersect: `public bool Intersect(Vector3 p0, Vector3 p1, out float t)` - Computes the intersection of the ray with the plane. Returns `true` if the ray intersects the plane, `false` otherwise. If the ray intersects the plane, the parameter `t` is set to the multiple of `p1` from the ray origin to the intersection point.
  - GetNormal: `public Vector3 GetNormal(Vector3 p1, Vector3 p)` - Computes the normal at the given point `p` on the plane assuming that `p1` is the direction of the ray that intersects the plane at `p`.

## Refelectance model

I use Phong reflectance model for computing the relative light coefficient. I use `BRDF.SimpleReflectance` method for computing the relative light coefficient for the diffuse and specular components.

## Scene

- Member variables:
  - `camera` (*camera*): Camera of the scene.
  - `solids` (*List<ISolid>*): List of solids in the scene.
  - `lights` (*List<ILightSource>*): List of light sources in the scene.
  - `materials` (*List<Material>*): List of materials in the scene.
- Methods:
  - ExampleScene: `public Scene ExampleScene()` - Creates sample scene from github repository.
  - RayCast: `public void RayCast()` - Computes the color of each pixel in the image using ray casting and saves the image to `demo.pfm`.
