// retirado de https://github.com/dotnet-ad/Transform/blob/9fba4493abcb8b6d0f7d3392f7641ab914c91c48/src/Transform/Transform2D.cs


using Chamboco.Engine.Entities.Components;

namespace Chamboco.Engine.Entities
{
    using Microsoft.Xna.Framework;
    using System.Collections.Generic;

    public partial class Transform : Component
    {
        #region Constructors

        public Transform()
        {
            this.Position = Vector2.Zero;
            this.Rotation = 0;
            this.Scale = Vector2.One;
        }

        #endregion

        #region Fields

        private Transform parent;

        private List<Transform> children = new ();

        private Matrix absolute, invertAbsolute, local;

        private float localRotation, absoluteRotation;

        private Vector2 localScale, absoluteScale, localPosition, absolutePosition;

        private bool needsAbsoluteUpdate = true, needsLocalUpdate = true;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the parent transform.
        /// </summary>
        /// <value>The parent.</value>
        public Transform Parent
        {
            get => this.parent;
            set
            {
                if(this.parent != value)
                {
                    if (this.parent != null)
                        this.parent.children.Remove(this);

                    this.parent = value;

                    if (this.parent != null)
                        this.parent.children.Add(this);

                    this.SetNeedsAbsoluteUpdate();
                }
            }
        }

        /// <summary>
        /// Gets all the children transform.
        /// </summary>
        /// <value>The children.</value>
        public IEnumerable<Transform> Children => this.children;

        /// <summary>
        /// Gets the absolute rotation.
        /// </summary>
        /// <value>The absolute rotation.</value>
        public float AbsoluteRotation => this.UpdateAbsoluteAndGet(ref this.absoluteRotation);

        /// <summary>
        /// Gets the absolute scale.
        /// </summary>
        /// <value>The absolute scale.</value>
        public Vector2 AbsoluteScale => this.UpdateAbsoluteAndGet(ref this.absoluteScale);

        /// <summary>
        /// Gets the absolute position.
        /// </summary>
        /// <value>The absolute position.</value>
        public Vector2 AbsolutePosition => this.UpdateAbsoluteAndGet(ref this.absolutePosition);

        /// <summary>
        /// Gets or sets the rotation (relative to the parent, absolute if no parent).
        /// </summary>
        /// <value>The rotation.</value>
        public float Rotation
        {
            get => this.localRotation;
            set
            {
                if (this.localRotation != value)
                {
                    this.localRotation = value;
                    this.SetNeedsLocalUpdate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the position (relative to the parent, absolute if no parent).
        /// </summary>
        /// <value>The position.</value>
        public Vector2 Position
        {
            get => this.localPosition;
            set
            {
                if(this.localPosition != value)
                {
                    this.localPosition = value;
                    this.SetNeedsLocalUpdate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the scale (relative to the parent, absolute if no parent).
        /// </summary>
        /// <value>The scale.</value>
        public Vector2 Scale
        {
            get => this.localScale;
            set
            {
                if (this.localScale != value)
                {
                    this.localScale = value;
                    this.SetNeedsLocalUpdate();
                    OnSetScale();
                }
            }
        }

        /// <summary>
        /// Gets the matrix representing the local transform.
        /// </summary>
        /// <value>The relative matrix.</value>
        public Matrix Local => this.UpdateLocalAndGet(ref this.absolute);

        /// <summary>
        /// Gets the matrix representing the absolute transform.
        /// </summary>
        /// <value>The absolute matrix.</value>
        public Matrix Absolute => this.UpdateAbsoluteAndGet(ref this.absolute);

        /// <summary>
        /// Gets the matrix representing the invert of the absolute transform.
        /// </summary>
        /// <value>The absolute matrix.</value>
        public Matrix InvertAbsolute => this.UpdateAbsoluteAndGet(ref this.invertAbsolute);

        #endregion

        #region Methods

        public void ToLocalPosition(ref Vector2 absolute, out Vector2 local)
        {
            Vector2.Transform(ref absolute, ref this.invertAbsolute, out local);
        }

        public void ToAbsolutePosition(ref Vector2 local, out Vector2 absolute)
        {
            Vector2.Transform(ref local, ref this.absolute, out absolute);
        }

        public Vector2 ToLocalPosition(Vector2 absolute)
        {
            Vector2 result;
            ToLocalPosition(ref absolute, out result);
            return result;
        }

        public Vector2 ToAbsolutePosition(Vector2 local)
        {
            Vector2 result;
            ToAbsolutePosition(ref local, out result);
            return result;
        }

        private void SetNeedsLocalUpdate()
        {
            this.needsLocalUpdate = true;
            this.SetNeedsAbsoluteUpdate();
        }

        private void SetNeedsAbsoluteUpdate()
        {
            this.needsAbsoluteUpdate = true;

            foreach (var child in this.children)
            {
                child.SetNeedsAbsoluteUpdate();
            }
        }

        private void UpdateLocal()
        {
            var result = Matrix.CreateScale(this.Scale.X, this.Scale.Y, 1);
            result *= Matrix.CreateRotationZ(this.Rotation);
            result *= Matrix.CreateTranslation(this.Position.X, this.Position.Y, 0);
            this.local = result;

            this.needsLocalUpdate = false;
        }

        private void UpdateAbsolute()
        {
            if (this.Parent == null)
            {
                this.absolute = this.local;
                this.absoluteScale = this.localScale;
                this.absoluteRotation = this.localRotation;
                this.absolutePosition = this.localPosition;
            }
            else
            {
                var parentAbsolute = this.Parent.Absolute;
                Matrix.Multiply(ref this.local, ref parentAbsolute, out this.absolute);
                this.absoluteScale = this.Parent.AbsoluteScale * this.Scale;
                this.absoluteRotation = this.Parent.AbsoluteRotation + this.Rotation;
                this.absolutePosition = Vector2.Zero;
                this.ToAbsolutePosition(ref this.absolutePosition, out this.absolutePosition);
            }

            Matrix.Invert(ref this.absolute, out this.invertAbsolute);

            this.needsAbsoluteUpdate = false;
        }

        private T UpdateLocalAndGet<T>(ref T field)
        {
            if (this.needsLocalUpdate)
            {
                this.UpdateLocal();
            }

            return field;
        }

        private T UpdateAbsoluteAndGet<T>(ref T field)
        {
            if (this.needsLocalUpdate)
            {
                this.UpdateLocal();
            }

            if (this.needsAbsoluteUpdate)
            {
                this.UpdateAbsolute();
            }

            return field;
        }

        #endregion

    }
}
