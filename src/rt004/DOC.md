Documentation
===

PerspectiveCamera
---

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
  - Constructor: `PerspectiveCamera(Vector3 center, Vector3 direction, Vector3 up, double nearPlaneDistance, double FOV, int imageWidth, int imageHeight)`
  - GetRay: `public bool GetRay(int x, int y, out Vector3 p0, out Vector3 p1)` - Computes the ray origin (`p0`) and direction (`p1`) for the given pixel coordinates. Returns `true` if the ray is valid, `false` otherwise. The ray is valid if the pixel coordinates are within the image bounds, i.e. $-1 \leq x \leq 1$ and $-1 \leq y \leq 1$.
