// MIT License
//
// Copyright (c) 2026 Alex "darbotron" Darby
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
using System.Collections;
using UnityEngine;

public class AngularVelocityChangeFromTorque : MonoBehaviour
{
    //------------------------------------------------------------------------
    void Start()
    {
        StartCoroutine( CoRunTest() );
    }

    //------------------------------------------------------------------------
    IEnumerator CoRunTest()
    {
		// WaitForFixedUpdate() yields until end of the FixedUpdate phase
		// (after all FixedUpdate, Unity physics update, and associated FixedUpdate animation updates etc.)
		// https://docs.unity3d.com/2022.3/Documentation/Manual/ExecutionOrder.html
		yield return new WaitForFixedUpdate();

		var rigidbody = GetComponentInChildren< Rigidbody >();

		//
		// setting rigidbody.inertiaTensor to pretty much any non-uniform value
		// causes most of the tests to fail
		//
		rigidbody.inertiaTensor = new Vector3( 2f, 2f, 2f );

		// haven't managed to get it to work with this yet so setting to identity for now :sweatsmile:
		rigidbody.inertiaTensorRotation = Quaternion.identity;


		// I know that drag & angulardrag would complicate this, so for now am setting them to 0 for the sake of this test
		rigidbody.drag        = 0f;
		rigidbody.angularDrag = 0f;

		//
		// TestOneFrameOfTorque() this resets the rigidbody's state, then adds a torque and waits til after the physics simulation to check the result
		//
		yield return TestOneFrameOfTorque( rigidbody, Quaternion.identity, torqueToApply: new Vector3( 3f, 0f, 0f ) );
		yield return TestOneFrameOfTorque( rigidbody, Quaternion.identity, torqueToApply: new Vector3( 0f, 3f, 0f ) );
		yield return TestOneFrameOfTorque( rigidbody, Quaternion.identity, torqueToApply: new Vector3( 0f, 0f, 3f ) );

		{
			var initialOrientation = Quaternion.Euler( 90f, 0f, 0f );

			yield return TestOneFrameOfTorque( rigidbody, initialOrientation, torqueToApply: new Vector3( 3f, 0f, 0f ) );
			yield return TestOneFrameOfTorque( rigidbody, initialOrientation, torqueToApply: new Vector3( 0f, 3f, 0f ) );
			yield return TestOneFrameOfTorque( rigidbody, initialOrientation, torqueToApply: new Vector3( 0f, 0f, 3f ) );
		}

		{
			var initialOrientation = Quaternion.Euler( 0f, 90f, 0f );

			yield return TestOneFrameOfTorque( rigidbody, initialOrientation, torqueToApply: new Vector3( 3f, 0f, 0f ) );
			yield return TestOneFrameOfTorque( rigidbody, initialOrientation, torqueToApply: new Vector3( 0f, 3f, 0f ) );
			yield return TestOneFrameOfTorque( rigidbody, initialOrientation, torqueToApply: new Vector3( 0f, 0f, 3f ) );
		}

		{
			var initialOrientation = Quaternion.Euler( 0f, 0f, 90f );

			yield return TestOneFrameOfTorque( rigidbody, initialOrientation, torqueToApply: new Vector3( 3f, 0f, 0f ) );
			yield return TestOneFrameOfTorque( rigidbody, initialOrientation, torqueToApply: new Vector3( 0f, 3f, 0f ) );
			yield return TestOneFrameOfTorque( rigidbody, initialOrientation, torqueToApply: new Vector3( 0f, 0f, 3f ) );
		}

		//
		// tests below here fail for pretty much any non-uniform inertia tensor ( 1, 1, 1 )
		// TL;DR: the way the intertia tensor works in Unity physics takes some liberties wrt actual physics.
		// see info on Unity Discussions: https://discussions.unity.com/t/calculating-the-angular-velocity-change-of-a-rigidbody-given-torque-and-timestep/1550314/4
		//

		{
			var initialOrientation = Quaternion.Euler( 90f, 90f, 0f );

			yield return TestOneFrameOfTorque( rigidbody, initialOrientation, torqueToApply: new Vector3( 3f, 0f, 0f ) );
			yield return TestOneFrameOfTorque( rigidbody, initialOrientation, torqueToApply: new Vector3( 0f, 3f, 0f ) );
			yield return TestOneFrameOfTorque( rigidbody, initialOrientation, torqueToApply: new Vector3( 0f, 0f, 3f ) );
		}

		{
			var initialOrientation = Quaternion.Euler( 0f, 90f, 90f );

			yield return TestOneFrameOfTorque( rigidbody, initialOrientation, torqueToApply: new Vector3( 3f, 0f, 0f ) );
			yield return TestOneFrameOfTorque( rigidbody, initialOrientation, torqueToApply: new Vector3( 0f, 3f, 0f ) );
			yield return TestOneFrameOfTorque( rigidbody, initialOrientation, torqueToApply: new Vector3( 0f, 0f, 3f ) );
		}

		{
			var initialOrientation = Quaternion.Euler( 90f, 0f, 90f );

			yield return TestOneFrameOfTorque( rigidbody, initialOrientation, torqueToApply: new Vector3( 3f, 0f, 0f ) );
			yield return TestOneFrameOfTorque( rigidbody, initialOrientation, torqueToApply: new Vector3( 0f, 3f, 0f ) );
			yield return TestOneFrameOfTorque( rigidbody, initialOrientation, torqueToApply: new Vector3( 0f, 0f, 3f ) );
		}

		{
			var initialOrientation = Quaternion.Euler( 10f, 57f, 140f );

			yield return TestOneFrameOfTorque( rigidbody, initialOrientation, torqueToApply: new Vector3( 3f, 0f, 0f ) );
			yield return TestOneFrameOfTorque( rigidbody, initialOrientation, torqueToApply: new Vector3( 0f, 3f, 0f ) );
			yield return TestOneFrameOfTorque( rigidbody, initialOrientation, torqueToApply: new Vector3( 0f, 0f, 3f ) );
		}

		//
		// rotate the inertia tensor...
		//

		{
			rigidbody.inertiaTensorRotation = Quaternion.Euler( 90f, 0f, 0f );

			yield return TestOneFrameOfTorque( rigidbody, Quaternion.identity, torqueToApply: new Vector3( 3f, 0f, 0f ) );
			yield return TestOneFrameOfTorque( rigidbody, Quaternion.identity, torqueToApply: new Vector3( 0f, 3f, 0f ) );
			yield return TestOneFrameOfTorque( rigidbody, Quaternion.identity, torqueToApply: new Vector3( 0f, 0f, 3f ) );
		}

		{
			rigidbody.inertiaTensorRotation = Quaternion.Euler( 0f, 90f, 0f );

			yield return TestOneFrameOfTorque( rigidbody, Quaternion.identity, torqueToApply: new Vector3( 3f, 0f, 0f ) );
			yield return TestOneFrameOfTorque( rigidbody, Quaternion.identity, torqueToApply: new Vector3( 0f, 3f, 0f ) );
			yield return TestOneFrameOfTorque( rigidbody, Quaternion.identity, torqueToApply: new Vector3( 0f, 0f, 3f ) );
		}

		{
			rigidbody.inertiaTensorRotation = Quaternion.Euler( 0f, 0f, 90f );

			yield return TestOneFrameOfTorque( rigidbody, Quaternion.identity, torqueToApply: new Vector3( 3f, 0f, 0f ) );
			yield return TestOneFrameOfTorque( rigidbody, Quaternion.identity, torqueToApply: new Vector3( 0f, 3f, 0f ) );
			yield return TestOneFrameOfTorque( rigidbody, Quaternion.identity, torqueToApply: new Vector3( 0f, 0f, 3f ) );
		}

		rigidbody.inertiaTensorRotation = Quaternion.identity;
    }

	//------------------------------------------------------------------------
	IEnumerator TestOneFrameOfTorque( Rigidbody rigidbody, Quaternion initialOrientation, Vector3 torqueToApply )
	{
		ResetRigidBody( rigidbody, initialOrientation );

		// checking assumptions
		Debug.Assert( rigidbody.velocity        == Vector3.zero );
		Debug.Assert( rigidbody.angularVelocity == Vector3.zero );

		rigidbody.AddTorque( torqueToApply );

		var accumulatedTorque = rigidbody.GetAccumulatedTorque();

		// checking assumptions
		Debug.Assert( accumulatedTorque == torqueToApply );

		var expectedAngularVelocity = CalculateRigidBodyAngularVelocityChange( rigidbody, accumulatedTorque, Time.fixedDeltaTime );

		// WaitForFixedUpdate() yields until end of the FixedUpdate phase
		// (after all FixedUpdate, Unity physics update, and associated FixedUpdate animation updates etc.)
		// https://docs.unity3d.com/2022.3/Documentation/Manual/ExecutionOrder.html
		yield return new WaitForFixedUpdate();

		var angularVelocityDeltaDifference = ( rigidbody.angularVelocity - expectedAngularVelocity );

		Debug.Log( $"initial orientation: {initialOrientation.eulerAngles} -- torque: {torqueToApply} -- actual ang.vel: {rigidbody.angularVelocity} -- expected ang.vel: {expectedAngularVelocity} -- difference: {angularVelocityDeltaDifference}" );

		// test fails if this fails
		const float k_accuracyThreshold = 0.000001f /*1.0e-6*/;

		Debug.Assert(	( Mathf.Abs( angularVelocityDeltaDifference.x ) < k_accuracyThreshold )
					&&	( Mathf.Abs( angularVelocityDeltaDifference.y ) < k_accuracyThreshold )
					&&	( Mathf.Abs( angularVelocityDeltaDifference.z ) < k_accuracyThreshold ),
					$"at least one component of angular velocity was more different than accuracy threshold from expected" );
	}

    //------------------------------------------------------------------------
    void ResetRigidBody( Rigidbody rigidbody, Quaternion initialOrientation )
    {
        Debug.Assert( rigidbody.GetAccumulatedForce()  == Vector3.zero );
        Debug.Assert( rigidbody.GetAccumulatedTorque() == Vector3.zero );

		rigidbody.velocity           = Vector3.zero;
		rigidbody.angularVelocity    = Vector3.zero;

		rigidbody.position           = Vector3.zero;
		rigidbody.transform.position = Vector3.zero;
		rigidbody.rotation           = initialOrientation;
        rigidbody.transform.rotation = initialOrientation;
    }

    //------------------------------------------------------------------------
    public static Vector3 CalculateRigidBodyAngularVelocityChange( Rigidbody rigidbody, Vector3 accumulatedTorqueWorldSpace, float timeStep )
	{
		//
		// context:
		// --------
		//
		// * rotational equivalent of f = ma has "moment of inertia" (MoI) rather than mass
		//		which in 3D is represented as a matrix
		//
		// * there's no matrix divide; the equivalent is multiply by inverse - so to get to angular acceleration
		//		from accumulater torque we need to multiply it by the inverse of the MoI matrix
		//
		// * https://docs.unity3d.com/2022.3/Documentation/ScriptReference/Matrix4x4.html
		// 		* Matrices in Unity are column major; i.e. the position of a transformation matrix is in
		//			the last column, and the first three columns contain x, y, and z-axes.
		// 		* Data is accessed as: row + (column*4).
		// 		* Matrices can be indexed like 2D arrays but note that in an expression like mat[a, b],
		// 			a refers to the row index, while b refers to the column index.
		//
		//
		// assumptions about Unity physics representations (which may be wrong?):
		// ----------------------------------------------------------------------
		// * in local space of a rigidbody, the MoI is a 3x3 matrix with Rigidbody.inertiaTensor in the diagonal
		// where there would be 1, 1, 1 in an identity matrix.
		//
		// * accumulated torque is in world space
		//
		// * Since accumulated torque is in world space, we have to convert the rigidbody's inverse MoI matrix into
		//		world space (i.e. same space as accumulated torque)

		// currently not taking account of manuallyScaledRigidbody.inertiaTensorRotation
		var moiTensorMatrixLocal = CalculateInertiaTensorMatrix( rigidbody.inertiaTensor, rigidbody.inertiaTensorRotation );

		// don't want any scale or translation here so generating a local -> world matrix from just rotation
		var localToWorldRotationMatrix = Matrix4x4.Rotate( rigidbody.transform.rotation );

		// need to include rigidbody.inertiaTensorRotation; but whenever I try to incorporate it I end up wildly off straight away
		var inertiaMatrixInverseWorldSpace = moiTensorMatrixLocal.inverse * localToWorldRotationMatrix;

		// there's no position in inertiaMatrixInverseWorldSpace so it _should_ be just rotation?
		var angularAcceleration  = inertiaMatrixInverseWorldSpace.MultiplyVector( accumulatedTorqueWorldSpace );
		var deltaAngularVelocity = angularAcceleration * timeStep;

		// seems rigidbody.angularVelocity is in Rigidbody local space?
		return rigidbody.transform.localToWorldMatrix.inverse.MultiplyVector( deltaAngularVelocity );
	}

	// see link for additional info, morelinks and explanation of how Physx stores & remakes the inertiatensor
	// https://discussions.unity.com/t/inertia-tensor-in-matrix-form-from-inertiatensor-and-inertiatensorrotation/802114/13
	//
	// note the tensor is 3x3 - will be in the 3x3 part of the returned 4x4
	public static Matrix4x4 CalculateInertiaTensorMatrix( Vector3 inertiaTensor, Quaternion inertiaTensorRotation )
	{
		var tensorRotationMatrix = Matrix4x4.Rotate( inertiaTensorRotation );
		var tensorDiagonalMatrix = Matrix4x4.Scale( inertiaTensor );          // scale basically puts inertiaTensor x,y,z into the diagonal

		return tensorRotationMatrix * tensorDiagonalMatrix * tensorRotationMatrix.transpose; // R is orthogonal, so R.transpose == R.inverse
	}
}
