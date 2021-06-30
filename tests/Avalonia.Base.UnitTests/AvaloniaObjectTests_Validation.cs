using System;
using System.Reactive.Subjects;
using Avalonia.Data;
using Xunit;

#nullable enable

namespace Avalonia.Base.UnitTests
{
    public class AvaloniaObjectTests_Validation
    {
        [Fact]
        public void Registration_Throws_If_DefaultValue_Fails_Validation()
        {
            Assert.Throws<ArgumentException>(() =>
                new StyledProperty<int>(
                    "BadDefault",
                    typeof(Class1),
                    new StyledPropertyMetadata<int>(101),
                    validate: Class1.ValidateFoo));
        }

        [Fact]
        public void Metadata_Override_Throws_If_DefaultValue_Fails_Validation()
        {
            Assert.Throws<ArgumentException>(() => Class1.FooProperty.OverrideDefaultValue<Class2>(101));
        }

        [Fact]
        public void SetValue_Throws_If_Fails_Validation()
        {
            var target = new Class1();

            Assert.Throws<ArgumentException>(() => target.SetValue(Class1.FooProperty, 101));
        }

        [Fact]
        public void SetValue_Throws_If_Fails_Validation_Attached()
        {
            var target = new Class1();

            Assert.Throws<ArgumentException>(() => target.SetValue(Class1.AttachedProperty, 101));
        }

        [Fact]
        public void Reverts_To_DefaultValue_If_LocalValue_Binding_Fails_Validation()
        {
            var target = new Class1();
            var source = new Subject<int>();

            target.Bind(Class1.FooProperty, source);
            source.OnNext(150);

            Assert.Equal(11, target.GetValue(Class1.FooProperty));
        }

        [Fact]
        public void Reverts_To_DefaultValue_If_Untyped_LocalValue_Binding_Fails_Validation()
        {
            var target = new Class1();
            var source = new Subject<object?>();

            target.Bind(Class1.FooProperty, source);
            source.OnNext(150);

            Assert.Equal(11, target.GetValue(Class1.FooProperty));
        }

        [Fact]
        public void Reverts_To_DefaultValue_If_Style_Binding_Fails_Validation()
        {
            var target = new Class1();
            var source = new Subject<int>();

            target.Bind(Class1.FooProperty, source, BindingPriority.Style);
            source.OnNext(150);

            Assert.Equal(11, target.GetValue(Class1.FooProperty));
        }

        [Fact]
        public void Reverts_To_DefaultValue_If_Untyped_Style_Binding_Fails_Validation()
        {
            var target = new Class1();
            var source = new Subject<object?>();

            target.Bind(Class1.FooProperty, source, BindingPriority.Style);
            source.OnNext(150);

            Assert.Equal(11, target.GetValue(Class1.FooProperty));
        }

        private class Class1 : AvaloniaObject
        {
            public static readonly StyledProperty<int> FooProperty =
                AvaloniaProperty.Register<Class1, int>(
                    "Qux",
                    defaultValue: 11,
                    validate: ValidateFoo);

            public static readonly AttachedProperty<int> AttachedProperty =
                AvaloniaProperty.RegisterAttached<Class1, Class1, int>(
                    "Attached",
                    defaultValue: 11,
                    validate: ValidateFoo);

            public static bool ValidateFoo(int value)
            {
                return value < 100;
            }
        }

        private class Class2 : AvaloniaObject
        {
        }
    }
}
