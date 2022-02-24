namespace SGLSharp {
	public interface IPoolable {
		bool OnEnpool();
		void OnDestroy();
		System.Type GetType { get; }
	}
}